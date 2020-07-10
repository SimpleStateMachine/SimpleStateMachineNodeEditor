


# StateMachineNodeEditor [![](https://img.shields.io/github/v/release/SimpleStateMachine/SimpleStateMachineNodeEditor)](https://github.com/SimpleStateMachine/SimpleStateMachineNodeEditor/releases) [![](https://img.shields.io/github/stars/SimpleStateMachine/SimpleStateMachineNodeEditor)](https://github.com/SimpleStateMachine/SimpleStateMachineNodeEditor) [![](https://img.shields.io/github/license/SimpleStateMachine/SimpleStateMachineNodeEditor)](https://github.com/SimpleStateMachine/SimpleStateMachineNodeEditor) [![](https://img.shields.io/badge/chat-slack-blueviolet.svg)](https://join.slack.com/t/simplestatemachine/shared_invite/zt-fnfhvvsx-fTejcpPn~PPb2ojdG_MQBg) [![](https://img.shields.io/badge/chat-telegram-blue.svg)](https://t.me/SimpleStateMachine)

 StateMachineNodeEditor is a WPF node-editor for visual work and editoring  state-machine
 
  ## Give a Star! :star:
If you like or are using this project please give it a star. Thanks!

 # Why SimpleStateMachine?
 Create state machine in **three** steps :
 
 **1.** Create scheme in  [this node editor🔗](https://github.com/SimpleStateMachine/SimpleStateMachineNodeEditor)

**2.** Load scheme in your project using [library📚](https://github.com/SimpleStateMachine/SimpleStateMachineLibrary) 

**3.** Describe your app logic and run the state machin🚘

## Features💡
### Custom Window with Visual Studio design
[![](https://github.com/SimpleStateMachine/SimpleStateMachineNodeEditor/blob/gh-pages/img/Custom%20window.jpg)](https://github.com/SimpleStateMachine/SimpleStateMachineNodeEditor)
### Two themes
[![](https://github.com/SimpleStateMachine/SimpleStateMachineNodeEditor/blob/gh-pages/img/Themes.jpg)](https://github.com/SimpleStateMachine/SimpleStateMachineNodeEditor)
### Two representations for State machine
* Scheme of nodes
* Table of transitions

### Validating 
* for unique name for Node/Transition
* for exists Nodes without connects
### Adding nodes and connections
[![](https://github.com/SimpleStateMachine/SimpleStateMachineNodeEditor/blob/gh-pages/img/adding.gif)](https://github.com/SimpleStateMachine/SimpleStateMachineNodeEditor)
### Undo and redo
[![](https://github.com/SimpleStateMachine/SimpleStateMachineNodeEditor/blob/gh-pages/img/undo%20and%20redo.gif)](https://github.com/SimpleStateMachine/SimpleStateMachineNodeEditor)
### Collapsing and  moving
[![](https://github.com/SimpleStateMachine/SimpleStateMachineNodeEditor/blob/gh-pages/img/collapsing%20and%20moving.gif)](https://github.com/SimpleStateMachine/SimpleStateMachineNodeEditor)
### Scaling
[![](https://github.com/SimpleStateMachine/SimpleStateMachineNodeEditor/blob/gh-pages/img/Scaling.gif)](https://github.com/SimpleStateMachine/SimpleStateMachineNodeEditor)
### Selection
[![](https://github.com/SimpleStateMachine/SimpleStateMachineNodeEditor/blob/gh-pages/img/selection.gif)](https://github.com/SimpleStateMachine/SimpleStateMachineNodeEditor)
### Naming for states and transitions
[![](https://github.com/SimpleStateMachine/SimpleStateMachineNodeEditor/blob/gh-pages/img/naming%20for%20states%20and%20transitions.gif)](https://github.com/SimpleStateMachine/SimpleStateMachineNodeEditor)
### Moving transitions
[![](https://github.com/SimpleStateMachine/SimpleStateMachineNodeEditor/blob/gh-pages/img/moving%20transitions.gif)](https://github.com/SimpleStateMachine/SimpleStateMachineNodeEditor)
### Deleting transitions
[![](https://github.com/SimpleStateMachine/SimpleStateMachineNodeEditor/blob/gh-pages/img/deleting%20transitions.gif)](https://github.com/SimpleStateMachine/SimpleStateMachineNodeEditor)
### Import/Export scheme from/to xml
```xml
<?xml version="1.0" encoding="utf-8"?>
<StateMachine>
  <States>
    <State Name="Start"/>
    <State Name="State 1"/>
    <State Name="State 2"/>
  </States>
  <StartState Name="Start" />
  <Transitions>
    <Transition Name="Transition 2" From="State 2" To="State 1" />
    <Transition Name="Transition 1" From="Start" To="State 2" />
  </Transitions>
   <Visualization>
    <State Name="Start" Position="37, 80" IsCollapse="False" />
    <State Name="State 1" Position="471, 195.54" IsCollapse="False" />
    <State Name="State 2" Position="276, 83.03999999999999" IsCollapse="False" />
  </Visualization>
</StateMachine>
```
### Save work space as PNG/JPEG
[![](https://github.com/SimpleStateMachine/SimpleStateMachineNodeEditor/blob/gh-pages/img/jpg.jpg)](https://github.com/SimpleStateMachine/SimpleStateMachineNodeEditor)

## Getting Started📂

 **1.** Download the [latest release archive](https://github.com/SimpleStateMachine/SimpleStateMachineNodeEditor/releases)
 
 **2.** Unzip the downloaded file
 
 **3.** Run the exe file
 

## Shortcuts📎
* <kbd>Ctrl</kbd></kbd> +<kbd>A</kbd> = Select All Nodes
* <kbd>Ctrl</kbd> + <kbd>S</kbd> = Save
* <kbd>Ctrl</kbd> + <kbd>Shift</kbd> + <kbd>S</kbd> = Save As
* <kbd>Ctrl</kbd> + <kbd>Shift</kbd> + <kbd>O</kbd> = Open
* <kbd>Ctrl</kbd> + <kbd>Shift</kbd> + <kbd>N</kbd> = New
* <kbd>Alt</kbd> + <kbd>F4</kbd> = Exit
* <kbd>Ctrl</kbd> + <kbd>Shift</kbd> + <kbd>Alt</kbd> + <kbd>P</kbd> = Export to PNG
* <kbd>Ctrl</kbd> + <kbd>Shift</kbd> + <kbd>Alt</kbd> + <kbd>J</kbd> = Export to JPEG
* <kbd>Ctrl</kbd> + <kbd>Z</kbd> = Undo
* <kbd>Ctrl</kbd> + <kbd>Y</kbd> = Redo
* <kbd>Ctrl</kbd> + <kbd>N</kbd> = Add Node
* <kbd>Ctrl</kbd> + <kbd>LMB</kbd> on Canvas = Start Select
* <kbd>Ctrl</kbd> + <kbd>LMB</kbd> on Connector = Create Loop
* <kbd>LMB</kbd> on Node = Select one Node
* <kbd>Ctrl</kbd> + <kbd>LMB</kbd> on Node = Select/UnSelect Node 
* <kbd>Ctrl</kbd> + <kbd>LMB</kbd> on Transition = Select/UnSelect Transition
* <kbd>Shift</kbd> + <kbd>LMB</kbd> on Transition = Multiple selection
* <kbd>LMB</kbd> on Connector = Start create Connect
* <kbd>Alt</kbd> + <kbd>LMB</kbd> on Connector = Move Connector in Node
* <kbd>Alt</kbd> + <kbd>LMB</kbd> on Canvas = Start Cut
* <kbd>Delete</kbd> = Delete Selected Elements
* <kbd>C</kbd> + <kbd>Delete</kbd> = Delete Selected Connector
* <kbd>N</kbd> + <kbd>Delete</kbd> = Delete Selected Nodes

## License📑

Copyright (c) SimpleStateMachine

Licensed under the [MIT](LICENSE) license.
