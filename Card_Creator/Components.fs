namespace Card_Creator

open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Layout
open System

module Components =

    let Labeling  (label:string, comp) = 
        StackPanel.create[
            StackPanel.spacing 5
            StackPanel.children[
                TextBlock.create [
                    TextBlock.text label 
                ]
                comp
            ]
        ]

    let LabelingCol (label:string, comp, col:int) = 
        StackPanel.create[
            StackPanel.spacing 5
            StackPanel.column(col)
            StackPanel.children[
                TextBlock.create [
                    TextBlock.text label 
                ]
                comp
            ]
        ]
