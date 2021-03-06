// ItemOptionsDialog.cs
//
// Author:
//   Lluis Sanchez Gual <lluis@novell.com>
//
// Copyright (c) 2008 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//

using System;
using Mono.Addins;
using MonoDevelop.Components;
using MonoDevelop.Projects.Extensions;
using MonoDevelop.Ide.Gui.Dialogs;
using MonoDevelop.Projects;

namespace MonoDevelop.Ide.Gui.Dialogs
{
	public class ItemOptionsDialog: OptionsDialog
	{
		public ItemOptionsDialog (): base ("/MonoDevelop/ProjectModel/Gui/ItemOptionPanels")
		{
		}
		
		public ItemOptionsDialog (Window parentWindow, object dataObject)
			: base (parentWindow, dataObject, "/MonoDevelop/ProjectModel/Gui/ItemOptionPanels")
		{
		}
		
		protected override void InitializeContext (ExtensionContext extensionContext)
		{
			base.InitializeContext (extensionContext);
			extensionContext.RegisterCondition ("ItemType", new ItemTypeCondition (DataObject.GetType ()));
			extensionContext.RegisterCondition ("ActiveLanguage", new ProjectLanguageCondition (DataObject));
			if (DataObject is Project) {
				extensionContext.RegisterCondition ("AppliesTo", new AppliesToCondition ((Project)DataObject));
				extensionContext.RegisterCondition ("FlavorType", new FlavorTypeCondition ((Project)DataObject));
				extensionContext.RegisterCondition ("ProjectTypeId", new ProjectTypeIdCondition ((Project)DataObject));
				extensionContext.RegisterCondition ("SupportsTarget", new SupportsTargetCondition ((Project)DataObject));
			} else {
				extensionContext.RegisterCondition ("AppliesTo", new FalseCondition ());
				extensionContext.RegisterCondition ("FlavorType", new FalseCondition ());
				extensionContext.RegisterCondition ("ProjectTypeId", new FalseCondition ());
			}
		}

		class FalseCondition: ConditionType
		{
			public override bool Evaluate (NodeElement conditionNode)
			{
				return false;
			}
		}
	}
}
