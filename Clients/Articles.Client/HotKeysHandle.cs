using Articles.Client.Shared;
using MudBlazor;
using Toolbelt.Blazor.HotKeys;

namespace Articles.Client;
public class HotKeysHandle
{
    private HotKeys _hotKeys = default!;
    private IDialogService DialogService = default!;
    private Stack<HotKeysContext> _hotKeysStack = new();

    public HotKeysHandle(HotKeys hotKeys, IDialogService dialogService)
    {
         _hotKeys = hotKeys;
        DialogService = dialogService;
        Init();
    }

    public HotKeysContext Add()
    {
        var context = _hotKeys.CreateContext();
        _hotKeysStack.Push(context);
        return context;
    }

    public void Remove()
    {
        var context = _hotKeysStack.Pop();
        context.Dispose();
    }

    public List<HotKeyEntry> GetHotKeys()
    {
        var hotkeys = _hotKeysStack
            .SelectMany(context => context.Keys)
            .ToList();
        return hotkeys;
    }

    public Exclude AllowsInputs = Exclude.InputNonText;
    public void Init()
    {        
        Add()
            .Add(ModKeys.Ctrl, Keys.F1, OpenDialog, "Hot Keys Info", AllowsInputs)
            .Add(ModKeys.Ctrl| ModKeys.Shift, Keys.F2, OpenDialog, "Test", AllowsInputs);
    }

    private void OpenDialog()
    {
        var options = new DialogOptions { 
            CloseOnEscapeKey = true,
            NoHeader = true,
            CloseButton = true
        };
        DialogService.Show<HotKeysInfoDialog>("Hot Keys", options);
    }
}
