# Light Cutter

![Light Cutter](HowToUse/images/LightCutter7.png) 

Light Cutter is a screenshot utility in Windows 3.1 and later, including Windows 11.
It can take screenshots of a rectangular area by using a mouse.
You can start this with shortcut key or delay.
Can capture the same area as last time, so you can capture it fast and continuously.

 ![Action Panel](HowToUse/images/ActionPanel71Dark.png)

 - [Light Cutter ver.7.1](https://github.com/surviveplus/Light-Cutter/releases)
 - [Light Cutter ver.5.4](
http://www.surviveplus.net/ja/archives/24
) ( Japanese Only )


In Japan, users of this software series are over 50,000 people total. For example, one of company approved it as official software.
However, the new version had not been released recently. So in this repository we migrate this and release new features.

## How to use 
- [How to use (Quick Start)](HowToUse/HowToUse.md)
- [How to Edit Action](HowToUse/HowToEditAction.md)

## Roadmap
### ver.7.0 - Migrating from previous version
Light Cutter ver.5.4 is Visual Basic 6.0 (unmanaged) based app with using .net COM classes (managed).
Even in Windows10, this version is limited but works.
First, we migrated from this version.
The latest version supports high DPI in addition to previous features.

### ver.7.1 - Split commands to support new targets
Before, we specified the area from only the full screen.
The latest version allowed you to automatically find specific apps and areas.

In addition to copying captured images to the Clipboard or saving them as images on the desktop, you can save these images on any folders. You can also trim the images on this workflow.

We wanted to be able to use these new features in combination with existing features. So, we re-designed the command to be split.
A new UI is provided for the new commands.
You can edit cutting flow using this UI.

### ver.7.2 - Smart (AI)
The new version will be smarter and easier to use.
Our friend always said, "I am glad that I can capture it as I thought more".
AI should be able to automatically determine capture scenarios and help more.


## Contributing
 - When you find bugs in [the released version](https://github.com/surviveplus/Light-Cutter/releases), report it to the [issues](https://github.com/surviveplus/Light-Cutter/issues).
 - The new feature will be prototyped first. 
 Look at the [issues](https://github.com/surviveplus/Light-Cutter/issues) and [code](https://github.com/surviveplus/Light-Cutter/tree/master/prototype) for the prototype and send a pull request if you have a good idea. 
 - Help to collect learning data. We still think about how we do this.

## License
Copyright (c) 1994-2023 Shin-ichi Koga. All rights reserved.

Licensed under the MIT License.