using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ILeoConsole.Localization;

namespace LeoConsole
{
    public class ENLocalisation : ILocalization
    {
        public string Language { get { return "en"; } }

        private string[,] _Dictionary;
        public string[,] Dictionary { get { return _Dictionary; } set { _Dictionary = value; } }

        public void DictionaryInit()
        {
            Dictionary = new string[,]
            {
                //First Startup

                {"lc_firstStartHi","Welcome to Leo Console!" },
                {"lc_firstStartInfo1","In order to use Leo Console, you need to create a root account." },
                {"lc_firstStartInfo2","To do this, enter 'newAccount'. Already have an account but it didn't load? Enter 'helpAccount'." },
                {"lc_firstStartHelp1","If you change your SavePath, Leo Console will no longer find your old Users.lcs file." },
                {"lc_firstStartHelp2","You can simply move your old Users.lcs file to the new SavePath" },

                //Start

                {"lc_starting", "Starting..."},
                {"lc_linuxUpdateSkip","Update skiped: LinuxBuild" },
                {"lc_loadUser","Loading: Users.lcs" },
                {"lc_loadUser404", "User.lcs could not be found!" },
                {"lc_loadUserSuc", "Users.lcs successfully loaded!" },

                //reload

                {"lc_loadPlugins","Loading: Plugins" },
                {"lc_loadPluginsSuc","plugins successfully loaded!" },
                {"lc_loadPluginsFailed","Plugins could not be loaded:" },
                {"lc_loadDatas","Register: Datas" },
                {"lc_loadDatasSuc","Datas successfully registered!" },
                {"lc_loadCommands","Register: Commands" },
                {"lc_loadCommandsSuc","Commands successfully registered!" },

                //Update

                {"lc_updateMsg","Updates available!" },
                {"lc_updateVCurrent","Current version: " },
                {"lc_updateVNew","New version: " },
                {"lc_updateQ","Should the update be downloaded? y/n" },
                {"lc_updateSkip","Continue without update!" },
                {"lc_updateNone","No updates found!" },
                {"lc_updateCheck404","The update page could not be reached! Please try again later." },
                {"lc_updateStart","Starting Update..." },
                {"lc_updateInstallFolder","Installation Folder: " },
                {"lc_updateStartDown","Start downloading the new version..." },
                {"lc_updateDownSuc","' successfully downloaded" },
                {"lc_updateUnZip","Extract zip file..." },
                {"lc_updateDataInfo","'data/' may not be compatible with the new version." },
                {"lc_updateDataQ","Should 'data/' be copied to the new version? y/n \n" },
                {"lc_updateDataCopySuc","'data/' successfully copied!" },
                {"lc_updateFail","Update could not be downloaded!" },

                //DefaultPlugins

                {"lc_dpkgUpdateStart","Searching for default plugins..." },
                {"lc_dpkgUpdateFail404","The default plugin page could not be reached! Please try again later." },
                {"lc_dpkgUpdateEndDown"," plugins installed. Please use 'reload' to activate there features." },
                {"lc_dpkgUpdateEndAll", "All default plugins installed!" },

                //User

                {"lc_userNew","\nTo create a new account, please enter the following information:" },
                {"lc_userNameTaken","This username is already taken!" },
                {"lc_userFinish","That's it. Press any key to save the account." },
                {"lc_userReboot","Press any key to restart LeoConsole" },
                {"lc_userDoesNotExist","This user could not be found!\n" },

                //Common Info

                {"lc_anyKeyContinue", "Press any key to continue..." },
                {"lc_complete","Complete!" },
                {"lc_cantFindCmdFront","The command '" },
                {"lc_cantFindCmdBack","' is either misspelled or could not be found." },
                {"lc_cantFindPathFront","The specified path '" },
                {"lc_cantFindPathBack","' is either misspelled or could not be found." },
                {"lc_fodDoesNotExist","File or directory does not exist!" },
                {"lc_username","Username" },
                {"lc_greeting","Greeting" },

                //Command Description

                {"lc_helpCmdInfo", "shows all registered commands and their descriptions" },
                {"lc_updateCmdInfo", "checks for updates to LeoConsole" },
                {"lc_exitCmdInfo", "quits LeoConsole" },
                {"lc_rebootCmdInfo","reboots LeoConsole" },
                {"lc_reloadCmdInfo"," reloads all plugins" },
                {"lc_creditsCmdInfo", "shows the credits" },
                {"lc_logoutCmdInfo","logs out the current user" },
                {"lc_newUserCmdInfo","creates a new User (root permission needed)" },
                {"lc_whoamiCmdInfo","shows shows all information about the logged in user" },
                {"lc_pkginfoCmdInfo","shows all installed packages and their descriptions" },
                {"lc_lsCmdInfo","Displays a list of files and directories" },
                {"lc_cdCmdInfo","cd" },
                {"lc_mkdirCmdInfo","create a directory" },
                {"lc_rmtrashCmdInfo","remove a file or a directory" },
                {"lc_langCmdInfo","sets the localisation language" },
                {"lc_dpkgCmdInfo","'default plugin' settings" },

                //Commands

                {"lc_rebootCmdDialog","Are you sure you want to restart Leo Console? Y/N" },
                {"lc_newUserCmdRootE","To create a new user, you need root rights!" },
                {"lc_mkdirCmdPropE","You need to provide at least one directory name!" },
                {"lc_rmtrashCmdPropE","You need to provide at least one directory or file name" },
                {"lc_rmtrashCmdDialogD","Are you sure you want to delete this directory and all of its contents?" },
                {"lc_rmtrashCmdDialogF","Are you sure you want to delete this file?" },
                {"lc_rmtrashCmdCanceled","deletion canceled" },
                {"lc_dpkgCmdDpkg","Default Plugins:" },
                {"lc_dpkgCmdDisabeled","Disabeled:" }
            };
        }
    }
}
