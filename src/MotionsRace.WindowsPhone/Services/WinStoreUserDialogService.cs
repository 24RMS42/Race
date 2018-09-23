using Acr.MvvmCross.Plugins.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace MotionsRace.WindowsPhone.Services
{
     public class WinStoreUserDialogService : AbstractUserDialogService
     {
         public override async void ActionSheet(ActionSheetConfig config)
         {             
             var msgBox = new PopupMenu();             

             foreach (var option in config.Options)
             {
                 var command = new UICommand(option.Text, new UICommandInvokedHandler((t) => option.Action.Invoke()));                 
                 msgBox.Commands.Add(command);
             }

             await msgBox.ShowAsync(new Windows.Foundation.Point(0,0));
         }

         public override void Alert(AlertConfig config)
         {
             throw new NotImplementedException();
         }

         public override void Confirm(ConfirmConfig config)
         {
             throw new NotImplementedException();
         }

         public override async  Task<bool> ConfirmAsync(ConfirmConfig config)
         {
             //var task = new TaskCompletionSource<bool>();             

             var msgDlg = new MessageDialog(config.Message);
             msgDlg.CancelCommandIndex = 1;
             msgDlg.Commands.Add(new UICommand(config.OkText, (s) => 
             { 
                // task.SetResult(true); 
             }));
             msgDlg.Commands.Add(new UICommand(config.CancelText, (s) => 
             { 
                // task.SetResult(false); 
             }));

             var result = await msgDlg.ShowAsync();
             return result.Label == config.OkText;
             //return task.Task;
         }

         protected override IProgressDialog CreateDialogInstance()
         {
             throw new NotImplementedException();
         }

         public override void Login(LoginConfig config)
         {
             throw new NotImplementedException();
         }

         public override void Prompt(PromptConfig config)
         {
             throw new NotImplementedException();
         }

         public override void Toast(string message, int timeoutSeconds = 3, Action onClick = null)
         {
             throw new NotImplementedException();
         }
     }
}
