// 
// SplitString.cs
//  
// Author:
//       Mike Krüger <mkrueger@novell.com>
// 
// Copyright (c) 2011 Novell, Inc (http://www.novell.com)
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

using System;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.PatternMatching;
using MonoDevelop.Projects.Dom;
using MonoDevelop.Core;
using System.Collections.Generic;
using Mono.TextEditor;
using System.Linq;
using MonoDevelop.Refactoring;

namespace MonoDevelop.CSharp.ContextAction
{
	public class SplitString : CSharpContextAction
	{
		protected override string GetMenuText (CSharpContext context)
		{
			return GettextCatalog.GetString ("Split string");
		}
		
		protected override bool IsValid (CSharpContext context)
		{
			if (context.Document.Editor.IsSomethingSelected)
				return false;
			var pExpr = context.GetNode<PrimitiveExpression> ();
			if (pExpr == null || !(pExpr.Value is string))
				return false;
			if (pExpr.LiteralValue.StartsWith ("@"))
				return pExpr.StartLocation < new AstLocation (context.Location.Line, context.Location.Column - 2) &&
					new AstLocation (context.Location.Line, context.Location.Column + 2) < pExpr.EndLocation;
			return pExpr.StartLocation < new AstLocation (context.Location.Line, context.Location.Column - 1) &&
				new AstLocation (context.Location.Line, context.Location.Column + 1) < pExpr.EndLocation;
		}
		
		protected override void Run (CSharpContext context)
		{
			var pExpr = context.GetNode<PrimitiveExpression> ();
			if (pExpr.LiteralValue.StartsWith ("@"))
				context.DoInsert (context.Document.Editor.LocationToOffset (context.Location.Line, context.Location.Column), "\" + @\"");
			context.DoInsert (context.Document.Editor.LocationToOffset (context.Location.Line, context.Location.Column), "\" + \"");
		}	
	}
}
