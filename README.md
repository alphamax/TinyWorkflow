# TinyWorkflow [![GitHub release](https://img.shields.io/github/release/alphamax/TinyWorkflow.svg?maxAge=2592000?style=flat-square)]() [![license](https://img.shields.io/github/license/alphamax/TinyWorkflow.svg?maxAge=2592000?style=flat-square)]()

## Definition

This is a little project for a simple but usefull workflow engine.
This workflow engine support While, Foreach, If, basic actions.

The project is provided with unit tests.

A technical blog post is available here :
[Full documentation](https://alphablog.org/a-little-workflow-engine/)

##Installation

Just download the last binaries and add it as reference to your project.
First workflow

Your first definition TinyWorkflow is like :
```
 Workflow<SimpleState> workflow = new Workflow<SimpleState>()
                .Do(EasyAction)
 workflow.Start(new SimpleState());
```
EasyAction is a method like :
```
public void EasyAction(SimpleState state)
{
    //Your code here
}
```
And SimpleState is a simple class :
```
public class SimpleState
{
    //Your statefull content here
}
```
## Now available in Nuget Package [Here](https://www.nuget.org/packages/Tiny-Workflow/2.1.0)
