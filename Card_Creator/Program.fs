namespace CounterApp

open Avalonia
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Layout
open Card_Creator.CardTypes
open Card_Creator.Components

module Main =

    let typesString =[
        "Normal";
        "Effect";
        "Fusion";
        "Synchro";
        "Link";
        "Ritual";
        "Xyz";
        "Pendulum";
        "Spell"
        "Trap";
    ]

    let monsterTypesStr = [
        "Aqua"
        "Beast"
        "Bird"
        "Bug"
        "Dragon"
        "Fairy"
        "Fiend"
        "Fish"
        "Insect"
        "Machine"
        "Plant"
        "Psychic"
        "Pyro"
        "Reptile"
        "Rock"
        "Sea Serpent"
        "Spellcaster"
        "Thunder"
        "Warrior"
        "Winged Beast"
        "Zombie"
                                        ]

    let view () =
        Component(fun ctx -> Grid.create [ 
            
            // let CardInfo = ctx.useState 
            let selectedCardType = ctx.useState ""
            let selectedMonsterType = ctx.useState ""
            
            let monsterVisible = ctx.useState false

            Grid.columnDefinitions "*,*"
            Grid.children [ 
                Border.create[
                    Border.column 0
                    Border.borderThickness(Thickness(10))
                    Border.child(TextBlock.create [
                        TextBlock.text "test"                        
                    ])
                    
                ] 

                Border.create [
                    Border.column 1
                    Border.borderThickness(Thickness(10))
                    Border.child( StackPanel.create[
                        StackPanel.spacing(10)
                        StackPanel.children [
                            
                            // imutavel
                            Grid.create [ 
                                Grid.columnDefinitions "*,*"
                                Grid.children [ 
                                    Border.create [
                                    Border.column 0
                                    Border.child(
                                        Labeling("Name:", TextBox.create [ 
                                     ]))
                                    ]
                                    Border.create [ 
                                    Border.column 1
                                    Border.child(
                                       Labeling("Type:", 
                                            ComboBox.create [
                                                ComboBox.minWidth(200)
                                                ComboBox.onSelectedIndexChanged(fun x -> 
                                                    selectedCardType.Set typesString.[ 
                                                        if x <= 0 then 0 else x
                                                    ] 
                                                    monsterVisible.Set (
                                                        if selectedCardType.Current <> "Spell" && selectedCardType.Current <> "Trap" then true else false
                                                    )
                                                ) 
                                                ComboBox.dataItems typesString
                                        ]
                                        )
                                     )
                                ] 
                                ]
                            ]

                             // Monsters
                            Grid.create [
                                Grid.isVisible monsterVisible.Current
                                Grid.columnDefinitions "*,*"
                                Grid.children [ 
                                    LabelingCol("Monster Type",ComboBox.create[
                                        ComboBox.minWidth(200)
                                        ComboBox.onSelectedIndexChanged(fun x -> selectedMonsterType.Set monsterTypesStr.[
                                            if x <= 0 then 0 else x
                                        ])
                                        ComboBox.dataItems monsterTypesStr
                                    ],0)

                                    LabelingCol("Level/Rank:", ComboBox.create[
                                        ComboBox.dataItems [
                                            "1"
                                            "2"
                                            "3"
                                            "4"
                                            "5"
                                            "6"
                                            "7"
                                            "8"
                                            "9"
                                            "10"
                                            "11"
                                            "12"
                                        ]                                                            
                                    ],1)
                                ]
                            ]
                          
                            Border.create [
                                Border.isVisible(not monsterVisible.Current)
                                Border.child(
                                    Labeling("Spell Type", ComboBox.create [
                                    ComboBox.isVisible (not monsterVisible.Current)
                                    ComboBox.dataItems [
                                        "Normal";
                                        "Quick";
                                        "Field";
                                        "Equip";
                                        "Counter";
                                        "Ritual";
                                        "Continuous"
                                     ]
                                    ]) 
                                )
                            ]
                    
                            // imutavel
                            Labeling("Description:", TextBox.create [ 
                                TextBox.minHeight(100)
                            ])
                        ]
                    ])
                ]
            
            ]
        ] )

type MainWindow() =
    inherit HostWindow()

    do
        base.Title <- "Yugioh Card Creator"
        base.MinWidth <- 500
        base.MinHeight <- 500
        base.Content <- Main.view ()


type App() =
    inherit Application()

    override this.Initialize() =
        this.Styles.Add(FluentTheme())
        this.RequestedThemeVariant <- Styling.ThemeVariant.Dark

    override this.OnFrameworkInitializationCompleted() =
        match this.ApplicationLifetime with
        | :? IClassicDesktopStyleApplicationLifetime as desktopLifetime -> desktopLifetime.MainWindow <- MainWindow()
        | _ -> ()

module Program =

    [<EntryPoint>]
    let main (args: string[]) =
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)
