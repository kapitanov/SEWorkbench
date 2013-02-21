NRefactory Code Completion Sample
=================================
This is a sample applications that shows how to do C# code completion in a text editor with NRefactory 5. We are using AvalonEdit as the text editor.

It's a pretty full featured sample containing ctrl+space like code completion and method parameter completion. Also showing nice images for different types in the completion window.

To get it to work properly I had to compile the latest verion of AvalonEdit, but compiled for VS 2010. The code can be found here:
https://github.com/lukebuehler/AvalonEdit

See:
 * https://github.com/icsharpcode/NRefactory
 * https://github.com/icsharpcode/SharpDevelop

![Screenshot](https://raw.github.com/lukebuehler/NRefactory-Completion-Sample/master/Doc/Screenshot.png)

Known Issues
=================================
This is a work in progress and so currently there are quite a few bugs/issues with the code completion. Hopefully they will be fixed soon.

  - completion window filter often doesnt show the best option, especially when starting a new word or using ctrl+space.
     for example: when typing "new" instead of suggesting as the best option "new" it shows: "NewsStyleUriParser". Or typing "var" becomes "Array"
     does this have something to do with the priorities if the AvalonEdit ICompletionData?

  - Extension methods do not work properly, we need to know if the method is called as an extension method or not
  - get xml documentation, what's the best way?
     maybe this?: https://github.com/icsharpcode/NRefactory/blob/master/ICSharpCode.NRefactory/Documentation/XmlDocumentationProvider.cs
  - show overloads in description of completionWindow

Attribution & License
=================================
Most of the code is extracted from different versions of SharpDevelop especially the newNR branch and NRefactory. So all the props go the the makers of NRefactory and SharpDevelop.
The completion icons come from SharpDevelop as well. 
The rest of the code was developed by lukebuehler and is released under the GNU LGPL license.

