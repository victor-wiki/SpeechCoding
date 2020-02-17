using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SpeechCoding
{
    public class UiHelper
    {
        public static void SetUI(Form form)
        {
            AppSetting setting = AppSettingManager.Setting;
            form.TopMost = setting.FormTopMost;
        }
    }
}
