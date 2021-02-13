#region [Copyright (c) 2021 ZeoFlow S.R.L.]
//	Distributed under the terms of an MIT-style license:
//
//	The MIT License
//
//	Copyright (c) 2021 ZeoFlow S.R.L.
//
//	Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
//	and associated documentation files (the "Software"), to deal in the Software without restriction, 
//	including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
//	and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
//	subject to the following conditions:
//
//	The above copyright notice and this permission notice shall be included in all copies or substantial 
//	portions of the Software.
//
//	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//	INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
//	PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
//	FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//	ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
#endregion
using UnityEngine;
using UnityEditor;
using UnityInputConverter;

namespace LuminosityEditor.IO
{
	public static partial class MenuCommands
	{
		[MenuItem("ZeoFlow/Input Manager/Create Input Manager", false, 2)]
		private static void CreateInputManager()
		{
			GameObject gameObject = new GameObject("Input Manager");
			gameObject.AddComponent<ZeoFlow.InputManager>();

			// Register Input Manager for undo, mark scene as dirty.
			Undo.RegisterCreatedObjectUndo(gameObject, "Create Input Manager");

			Selection.activeGameObject = gameObject;
		}

		[MenuItem("ZeoFlow/Input Manager/Convert Unity Input", false, 5)]
		private static void ConvertInput()
		{
			string sourcePath = EditorUtility.OpenFilePanel("Select Unity input settings asset", "", "asset");
			if(!string.IsNullOrEmpty(sourcePath))
			{
				string destinationPath = EditorUtility.SaveFilePanel("Save imported input axes", "", "input_manager", "xml");
				if(!string.IsNullOrEmpty(destinationPath))
				{
					try
					{
						InputConverter converter = new InputConverter();
						converter.ConvertUnityInputManager(sourcePath, destinationPath);

						EditorUtility.DisplayDialog("Success", "Unity input converted successfuly!", "OK");
					}
					catch(System.Exception ex)
					{
						Debug.LogException(ex);

						string message = "Failed to convert Unity input! Please make sure 'InputManager.asset' is serialized as a YAML text file.";
						EditorUtility.DisplayDialog("Error", message, "OK");
					}
				}
			}
		}

		[MenuItem("ZeoFlow/Input Manager/Check For Updates", false, 400)]
        public static void CheckForUpdates()
        {
            Application.OpenURL("https://github.com/zeoflow/InputManager");
        }

		[MenuItem("ZeoFlow/Input Manager/Report Bug", false, 402)]
		public static void OpenReportBugPage()
		{
			Application.OpenURL("https://github.com/zeoflow/InputManager/issues");
		}

        [MenuItem("ZeoFlow/Input Manager/Contact", false, 403)]
        public static void OpenContactDialog()
        {
            string message = "Email: github@zeoflow.com";
            EditorUtility.DisplayDialog("Contact", message, "Close");
        }

		[MenuItem("ZeoFlow/Input Manager/About", false, 405)]
		public static void OpenAboutDialog()
		{
			string message = "Input Manager, MIT Licensed\nCopyright \u00A9 2021 ZeoFlow S.R.L.\nhttps://github.com/zeoflow";
			EditorUtility.DisplayDialog("About", message, "OK");
		}
	}
}
