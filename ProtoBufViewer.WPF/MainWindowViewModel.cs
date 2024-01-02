using CommunityToolkit.Mvvm.Input;
using Google.Protobuf;
using Microsoft.Win32;
using ProtoBuf.Logic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using static ProtoBuf.Antlr.Protobuf3Parser;

namespace ProtoBufViewer.WPF;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private ProtoContext? parseResult;
    private MessageViewModel? selectedMessage;

    public MainWindowViewModel()
    {
        OpenProtoCommand = new RelayCommand(OpenProto);
        OpenBinaryCommand = new RelayCommand(OpenBinary, () => SelectedMessage != null);
        Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() =>
        {
            if (!string.IsNullOrWhiteSpace(Settings.Default.ProtoFile))
            {
                ProtoFile = new(Settings.Default.ProtoFile);
                SelectedMessage = Messages.FirstOrDefault(x => x.Name == Settings.Default.SelectedMessage);
                if (SelectedMessage != null && Settings.Default.ProtoBinFile is { } binFile)
                {
                    ProtoBinFile = new(binFile);
                }
            }
        }));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public RelayCommand OpenProtoCommand { get; }
    public RelayCommand OpenBinaryCommand { get; }

    public ObservableCollection<MessageViewModel> Messages { get; } = [];
    public MessageViewModel? SelectedMessage
    {
        get => selectedMessage;
        set
        {
            selectedMessage = value;
            OnPropertyChanged();
            OpenBinaryCommand.NotifyCanExecuteChanged();
            Settings.Default.SelectedMessage = value?.Name;
            Settings.Default.Save();
        }
    }

    protected ProtoContext? ParseResult { get => parseResult; set { parseResult = value; OnPropertyChanged(); } }


    private IReadOnlyCollection<TypedMessage>? typedMessages;
    private string? searchText;

    public IReadOnlyCollection<TypedMessage>? TypedMessages { get => typedMessages; set { typedMessages = value; OnPropertyChanged(); } }
    public FileInfo? ProtoFile
    {
        get => string.IsNullOrWhiteSpace(Settings.Default.ProtoFile) ? null : new(Settings.Default.ProtoFile);
        set
        {
            Settings.Default.ProtoFile = value?.FullName;
            Settings.Default.Save();
            OnPropertyChanged();
            ParseProto();
        }
    }
    public FileInfo? ProtoBinFile
    {
        get => string.IsNullOrWhiteSpace(Settings.Default.ProtoBinFile) ? null : new(Settings.Default.ProtoBinFile);
        set
        {
            Settings.Default.ProtoBinFile = value?.FullName;
            Settings.Default.Save();
            OnPropertyChanged();
            ParseBinary();
        }
    }

    public Action<IEnumerable<ProtoType>>? SelectTypedMessage { get; internal set; }

    private void OpenProto()
    {
        OpenFileDialog openFileDialog = new()
        {
            Filter = "ProtoBuf Files (*.proto)|*.proto"
        };
        if (openFileDialog.ShowDialog() is true)
        {
            ProtoFile = new(openFileDialog.FileName);
        }
    }

    private void ParseProto()
    {
        try
        {
            Messages.Clear();
            if (ProtoFile is { Exists: true, FullName: { } file })
            {
                ParseResult = ProtoParser.ParseFile(file);
                var visitor = new MessageViewModel.Visitor();
                visitor.Visit(ParseResult);
                foreach (var m in visitor.Messages.Where(x => x.Parent == null))
                {
                    Messages.Add(m);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void OpenBinary()
    {
        if (SelectedMessage == null || ParseResult == null)
        {
            return;
        }

        OpenFileDialog openFileDialog = new()
        {
            Filter = "ProtoBuf Binary Files (*.pb)|*.pb"
        };
        if (openFileDialog.ShowDialog() is true)
        {
            ProtoBinFile = new(openFileDialog.FileName);
        }
    }

    private async Task ParseBinary()
    {
        TypedMessages = null;
        try
        {
            if (ProtoBinFile is { Exists: true, FullName: { } file }
                && ParseResult is { } protoContext
                && SelectedMessage is { MessageDefContext: { } messageDefContext })
            {
                using var fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                using var coded = CodedInputStream.CreateWithLimits(fs, int.MaxValue, int.MaxValue);
                var decoder = new TypedMessageDecoder();
                TypedMessages = await decoder.Parse(coded, async _ => { }, protoContext, messageDefContext);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    public string? SearchText
    {
        get => searchText;
        set
        {
            searchText = value;
            DoSearch();
            OnPropertyChanged();
        }
    }

    private void DoSearch()
    {
        var stack = new Stack<ProtoType>();
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            return;
        }
        foreach (var m in TypedMessages ?? Enumerable.Empty<TypedMessage>())
        {
            stack.Push(m);
            if (FindMessage(stack, SearchText))
            {
                SelectTypedMessage?.Invoke(stack.Reverse());
                return;
            }
            stack.Pop();
        }

        static bool FindMessage(Stack<ProtoType> stack, string searchText)
        {
            var current = stack.Peek();
            switch (current)
            {
                case TypedString s when s.Value.Contains(searchText, StringComparison.OrdinalIgnoreCase): return true;
                case TypedBool b when searchText.Equals("true", StringComparison.OrdinalIgnoreCase) && b.Value: return true;
                case TypedBool b when searchText.Equals("false", StringComparison.OrdinalIgnoreCase) && !b.Value: return true;
                case TypedEnum e when e.EnumValue?.Contains(searchText) == true: return true;
                case TypedUnknown u when u.Value is string s && s.Contains(searchText): return true;
                case TypedMessage m:
                    foreach (var f in m.Fields)
                    {
                        stack.Push(f.Value);
                        if (FindMessage(stack, searchText))
                        {
                            return true;
                        }
                        stack.Pop();
                    }
                    break;
            }
            return false;
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}