namespace CounterApp

open Avalonia
open Avalonia.Media
open Avalonia.Media.Imaging
open Avalonia.Controls.ApplicationLifetimes
open Avalonia.Themes.Fluent
open Avalonia.FuncUI.Hosts
open Avalonia.Controls
open Avalonia.FuncUI
open Avalonia.FuncUI.DSL
open Avalonia.Layout
open Avalonia.Platform
open Avalonia.Platform.Storage
open Avalonia.Threading
open Card_Creator.CardTypes
open Card_Creator.Components


type State =
    { isVisible: bool
      name: string
      cardType: CardType
      attack: uint
      defence: uint
      level: uint
      atribute: attributes
      SpellType: string
     }

type Msg =
    | SetName of string
    | SetCardType of CardType
    | SetAttack of uint
    | SetDefence of uint
    | SetLevel of uint
    | SetAtribute of attributes
    | SetSpellType of string


module Main =
    open System

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

    let filter = FilePickerOpenOptions(
        AllowMultiple = false
        )

    let view () =
        Component(fun ctx -> Grid.create [ 
               
            // let CardInfo = ctx.useState 
            let selectedCardType = ctx.useState ""
            let selectedMonsterType = ctx.useState ""
            let isVisible = ctx.useState false
            
            let name = ctx.useState ""
            let cardType = ctx.useState ""
            let attack = ctx.useState 0
            let defence = ctx.useState 0
            let level = ctx.useState 0
            let atribute = ctx.useState ""
            let spellType = ctx.useState ""
            let imgPath = ctx.useState ""

            let setName = fun x -> name.Set x
            let setCardType = fun x -> 
                selectedCardType.Set typesString.[ if x <= 0 then 0 else x ]
                isVisible.Set (
                    if selectedCardType.Current <> "Spell" && selectedCardType.Current <> "Trap" then true else     false
                ) 

            let setAttack = fun x -> attack.Set x
            let setDefence = fun x -> defence.Set x
            let setLevel = fun x -> level.Set x
            let setAtribute = fun x -> atribute.Set x
            let setSpellType = fun x -> spellType.Set x
           
            let top = TopLevel.GetTopLevel ctx.control
            
            let openFile() = 
                async{
                    let filter = FilePickerFileType("Image types")
                    filter.Patterns <- [|"*.png";"*.jpeg";"*.jpg"|]

                    let options = FilePickerOpenOptions(
                        AllowMultiple = false  ,
                        Title = "Select an image",
                        FileTypeFilter = [|filter|]
                        )

                    let! file = 
                        top.StorageProvider.OpenFilePickerAsync(options)
                        |>Async.AwaitTask 

                    printfn $"{file.[0].Path}"
                }
            
            let imagePath _ =
                openFile()
                |>Async.StartImmediate

            let saveCard = fun _ -> (printfn $"{name.Current}")

            let aaa = new Bitmap(AssetLoader.Open(Uri("avares://Card_Creator/assets/cardTemplates/Normal.jpeg")))
            let brush = new ImageBrush(aaa)

            Grid.columnDefinitions "*,*"
            Grid.children [ 
                Border.create[
                    Border.column 0
                    Border.borderThickness(Thickness(50))
                    Border.onPointerPressed imagePath
                    Border.child(Grid.create[
                        Grid.height 500
                        // Grid.background Media.Brushes.MediumSlateBlue
                        Grid.background  brush
                        Grid.children [
                           
                        ]
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
                                            TextBox.onTextChanged(setName)
                                     ]))
                                    ]
                                    Border.create [ 
                                    Border.column 1
                                    Border.child(
                                       Labeling("Type:", 
                                            ComboBox.create [
                                                ComboBox.minWidth(200)
                                                ComboBox.onSelectedIndexChanged(setCardType) 
                                                ComboBox.dataItems typesString
                                            ]
                                        )
                                     )
                                ] 
                                ]
                            ]

                             // Monsters
                            Grid.create [
                                Grid.isVisible isVisible.Current
                                Grid.columnDefinitions "*,*"
                                Grid.children [ 
                                    LabelingCol("Monster Type", TextBox.create [
                                        TextBox.onTextChanged(setSpellType)
                                    ] ,0)

                                    LabelingCol("Level/Rank:", ComboBox.create[
                                        ComboBox.onSelectedIndexChanged(setLevel)
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
                                Border.isVisible(not isVisible.Current)
                                Border.child(
                                    Labeling("Spell Type", ComboBox.create [
                                        // ComboBox.onSelectedIndexChanged(setSpellType)
                                    ComboBox.isVisible (not isVisible.Current)
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

                            Button.create [
                                Button.content "Save card"
                                Button.onClick saveCard
                            ]
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
        // Card_Creator.CardMaker.handle
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .UseSkia()
            .StartWithClassicDesktopLifetime(args)
