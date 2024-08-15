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
       // "Pendulum";
        "Spell"
        "Trap";
    ]
    
    let attributes = [
        "Dark"
        "Earth"
        "Light"
        "Water"
        "Wind"
        "Fire"
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
            let preview = ctx.useState false
            let name = ctx.useState ""
            let cardType = ctx.useState ""
            let description = ctx.useState ""
            let attack = ctx.useState ""
            let defence = ctx.useState ""
            let level = ctx.useState 0
            let atribute = ctx.useState ""
            let spellType = ctx.useState ""
            let imgPath = ctx.useState ""
            let linkArrows = ctx.useState { 
                left = false; 
                right = false; 
                top = false; 
                bottom = false; 
                topLeft = false; 
                topRight = false; 
                bottomRight = false; 
                bottomLeft = false
            }

            let setCardTypeTemplate (currentTemplate: string) =
                let templatePath = TemplateAssets.[ toEnum currentTemplate ]
                let file = new Bitmap(AssetLoader.Open(Uri(templatePath)))
                new ImageBrush(file)
            
            let setMonsterAttr (x: string) = 
                let atr = attributesAssets.[attrToEnum x]
                new Bitmap(AssetLoader.Open(Uri(atr)))
            
            let setCardImage (x: Uri option) =
                if x.IsSome then
                    new Bitmap(x.Value.AbsolutePath)
                else 
                    new Bitmap(AssetLoader.Open(Uri("avares://Card_Creator/assets/oliver.jpeg")))

            let cardBrush =ctx.useState( setCardTypeTemplate("Effect"))
           
            let attributesImage = ctx.useState( setMonsterAttr("Fire"))
            
            let cardImage = ctx.useState( setCardImage None)
            
            let isLink = ctx.useState false
            
            let setCardType = fun x -> 
                selectedCardType.Set typesString.[ if x <= 0 then 0 else x ]
                isVisible.Set (
                    if selectedCardType.Current <> "Spell" && selectedCardType.Current <> "Trap" then true else     false
                ) 
                
                if selectedCardType.Current = "Link" then
                    isLink.Set true
                else 
                    isLink.Set false
                
                preview.Set( not (String.IsNullOrWhiteSpace(selectedCardType.Current)) && x <> -1)
                cardBrush.Set(setCardTypeTemplate selectedCardType.Current)
            
            let setAtribute  = fun x -> 
                atribute.Set attributes.[ if x <= 0 then 0 else x ]
                attributesImage.Set (setMonsterAttr atribute.Current)

            let setLinkArrows(col:int, row:int, chk:bool) =
                let arrows:LinkArrows = {
                    left = if (row = 1 && col = 0) then chk else linkArrows.Current.left
                    right = if (row = 1 && col = 3) then chk else linkArrows.Current.right
                    top = if (row = 0 && col = 1) then chk else linkArrows.Current.top
                    bottom = if (row = 3 && col = 1) then chk else linkArrows.Current.bottom
                    topLeft = if (row = 0 && col = 0) then chk else linkArrows.Current.topLeft
                    topRight = if (row = 0 && col = 3) then chk else linkArrows.Current.topRight
                    bottomLeft = if (row = 3 && col = 0) then chk else linkArrows.Current.bottomLeft
                    bottomRight = if (row = 3 && col = 3) then chk else linkArrows.Current.bottomRight
                }
                printfn  $"\n{col},{row}\n"
                linkArrows.Set arrows
            
            let setName = fun x -> name.Set x
            let setAttack = fun x -> attack.Set x
            let setDefence = fun x -> defence.Set x
            let setLevel = fun x -> level.Set x
            let setAttack = fun x -> attack.Set x
            let setDefence = fun x -> defence.Set x
            let setSpellType = fun x -> spellType.Set x
            let setDescription = fun x -> description.Set x
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
                    
                    // printfn $"{file.[0].Path}"
                    if file.Count > 0 then
                        let fileUri = file.[0].Path
                        imgPath.Set (fileUri.AbsolutePath)
                        cardImage.Set (setCardImage (Some fileUri))

                }
            

            let setImagePath _ =
                openFile() 
                |>Async.StartImmediate
                // imgPath.Set (file.ToString())

            let attr = new Bitmap(AssetLoader.Open(Uri("avares://Card_Creator/assets/attributes/FIRE.png")))
            let levelBitmap = new Bitmap(AssetLoader.Open(Uri("avares://Card_Creator/assets/Level.png")))
            
            let saving = ctx.useState false 

            let getCard = fun _ ->
                if selectedCardType.Current = "Spell" || selectedCardType.Current = "Trap" then
                    let card:Card = {
                        name = name.Current
                        description = if String.IsNullOrWhiteSpace(description.Current) then " " else description.Current
                        cardType = toEnum selectedCardType.Current
                        image = imgPath.Current
                        monster = None
                    }
                    card
                else
                    let monster:Monster ={
                        attack = attack.Current|> int
                        defence =  if isLink.Current then 0 else defence.Current|> int
                        level =  if isLink.Current then 1 else level.Current|> int
                        atribute = attrToEnum atribute.Current
                        Type = spellType.Current
                        linkArrows = if isLink.Current then Some linkArrows.Current else None
                        }
                    let card:Card = {
                        name = name.Current
                        description = if String.IsNullOrWhiteSpace(description.Current) then " " else description.Current
                        cardType = toEnum selectedCardType.Current
                        image = imgPath.Current
                        monster = Some monster
                    }
                    card
 
            let saveCard = fun x ->
                if not saving.Current  then
                    saving.Set true
                    try
                        let card:Card = getCard()
                        
                        printfn $@"top: {linkArrows.Current.top}, bottom: {linkArrows.Current.bottom}, \n 
                        left:{linkArrows.Current.left}, right: {linkArrows.Current.right} \n
                        topLeft: {linkArrows.Current.topLeft}, topRight: {linkArrows.Current.topRight}, \n
                        bottomLeft: {linkArrows.Current.bottomLeft}, bottomRight: {linkArrows.Current.bottomRight}
                            "
                        Card_Creator.CardMaker.handle (Some card)
                        SukiUI.Controls.SukiHost.ShowToast($"{name.Current} foi salvo!", "", TimeSpan.FromSeconds(5), fun x -> printfn("Toast clicked !"))
                        saving.Set false

                        // Environment.Exit 0
                    with
                    | :? System.DivideByZeroException -> saving.Set false
                
            Grid.columnDefinitions "*,*"
            Grid.children [                          
                Border.create [
                    Border.column 0
                    Border.isVisible preview.Current
                    Border.isEnabled preview.Current
                    Border.borderThickness(Thickness(50))
                    Border.child(Grid.create[
                        Grid.height 500
                        Grid.width 300
                        Grid.rowDefinitions "60,20,250,130,*"
                        Grid.background cardBrush.Current
                        Grid.children [   
                            Grid.create[
                                Grid.row 0
                                Grid.width 300
                                Grid.height 20
                                Grid.margin (Thickness(25,52,19,0))
                                Grid.columnDefinitions "178,*"
                                Grid.children [
                                    // Card Name
                                    TextBlock.create[
                                        TextBlock.column 0
                                        TextBlock.text name.Current
                                        TextBlock.margin(Thickness(0,2))
                                        TextBlock.fontSize 16
                                        TextBlock.horizontalAlignment HorizontalAlignment.Left
                                        TextBlock.foreground (
                                            if selectedCardType.Current  = "Spell" || selectedCardType.Current = "Trap" || selectedCardType.Current = "Xyz" then 
                                                Media.Brushes.WhiteSmoke
                                            else 
                                                Media.Brushes.Black
                                            )
                                    ]

                                    // Card Attribute
                                    Image.create [ 
                                        Image.column 1
                                        Image.source attributesImage.Current 
                                        Image.isVisible isVisible.Current
                                        // Image.margin(25,55)
                                        Image.height 30
                                    ]
                                ]
                            ]
                            // Card Level 12 levels
                            Grid.create [
                                Grid.row 1
                                Grid.isVisible isVisible.Current
                                Grid.width 300
                                Grid.height 10
                                Grid.horizontalAlignment  (if cardType.Current = "Xyz" then HorizontalAlignment.Right else HorizontalAlignment.Left)
                                Grid.columnDefinitions "20,20,20,20,20,20,20,20,20,20,20,20"
                                Grid.margin (Thickness(20, 15,0,0))
                                Grid.children [
                                    if level.Current > 0 then
                                        for i in 0.. (level.Current-1) do
                                            Image.create[
                                                Image.column (12 - i - 1)
                                                Image.height 20
                                                Image.source levelBitmap
                                            ]
                                ]
                            ]
                            
                            Grid.create [
                                Grid.row 2
                                Grid.height 250
                                Grid.width 300
                                Grid.opacity 0.9
                                Grid.children[ 
                                    Border.create[
                                        Border.borderThickness(Thickness(14,12,56,8))
                                        Border.width 300
                                        Border.child(
                                            Image.create[
                                                Image.stretch Stretch.Fill
                                                Image.source cardImage.Current
                                            ]
                                        )
                                    ]
                                ]
                            ]

                            Grid.create [
                                Grid.row 3
                                // Grid.height 35
                                Grid.margin(Thickness(24,10,66,20))
                                Grid.rowDefinitions "10,*,33"
                                Grid.children [
                                    TextBlock.create [
                                        TextBlock.row 0
                                        TextBlock.isVisible isVisible.Current
                                        TextBlock.fontWeight FontWeight.Bold
                                        TextBlock.foreground Media.Brushes.Black
                                        TextBlock.fontSize 10
                                        TextBlock.text $"[{spellType.Current}]"
                                    ]
                                    TextBlock.create [ 
                                        TextBlock.row 1
                                        TextBlock.width 258
                                        TextBlock.margin(0,2,0,0)
                                        TextBlock.foreground Media.Brushes.Black
                                        TextBlock.fontSize 10
                                        TextBlock.lineHeight 11
                                        TextBlock.textAlignment TextAlignment.Justify
                                        TextBlock.textWrapping Avalonia.Media.TextWrapping.Wrap
                                        TextBlock.text description.Current
                                    ]

                                    StackPanel.create [
                                        StackPanel.row 2
                                        StackPanel.opacity 0.8
                                        StackPanel.margin(Thickness(165,3,0,0))
                                        // StackPanel.background Media.Brushes.MediumSlateBlue
                                        StackPanel.orientation Orientation.Horizontal
                                        StackPanel.children [
                                            TextBlock.create [
                                                TextBlock.foreground Media.Brushes.Black
                                                TextBlock.fontSize 12
                                                TextBlock.margin(Thickness(4,0,0,0))
                                                TextBlock.fontWeight FontWeight.Bold
                                                TextBlock.text attack.Current
                                            ]
                                            TextBlock.create [
                                                TextBlock.foreground Media.Brushes.Black
                                                TextBlock.fontSize 12
                                                TextBlock.margin(Thickness(28,0,0,0))
                                                TextBlock.fontWeight FontWeight.Bold
                                                TextBlock.text defence.Current
                                            ]
                                        ]
                                    ]
                                ]
                            ]
                            Button.create [ 
                                Button.row 4
                                Button.margin(Thickness(0,0,40,0))
                                Button.content "Select an image"
                                // Button.verticalContentAlignment VerticalAlignment.Center
                                // Button.horizontalContentAlignment HorizontalAlignment.Center
                                Button.width 300
                                Button.onClick setImagePath
                            ]
                        ]
                    ])
                   
                ] 

                // Right side - Form

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
                            
                            StackPanel.create [ 
                                StackPanel.isVisible isVisible.Current
                                StackPanel.spacing 5
                                StackPanel.children [
                                    TextBlock.create [
                                        TextBlock.text "Attribute:"
                                    ]
                                    ComboBox.create[
                                        ComboBox.onSelectedIndexChanged setAtribute 
                                        ComboBox.dataItems attributes
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
                                            "0"
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
                            
                            // link Rate
                            StackPanel.create [
                                StackPanel.isVisible isLink.Current
                                StackPanel.horizontalAlignment HorizontalAlignment.Center
                                StackPanel.verticalAlignment VerticalAlignment.Center
                                StackPanel.orientation Orientation.Vertical
                                StackPanel.children [
                                    TextBlock.create[
                                        TextBlock.text "Link Rate:"
                                        TextBlock.fontWeight FontWeight.Bold
                                        TextBlock.fontSize 12
                                        TextBlock.margin(Thickness(0,0,0,5))

                                    ]
                                    Grid.create [ 
                                        Grid.columnDefinitions "*,*,*"
                                        Grid.rowDefinitions "*,*,*"
                                        Grid.children[
                                            for i in 0..3 do
                                                for j in 0..3 do
                                                    if i=1 && j=1 then
                                                        TextBox.create [TextBox.isVisible false]
                                                    else
                                                        CheckBox.create [
                                                            CheckBox.row i
                                                            CheckBox.column j
                                                            CheckBox.onChecked(fun _ -> setLinkArrows(j,i,true))
                                                            CheckBox.onUnchecked(fun _ -> setLinkArrows(j,i,false))
                                                        ]
                                        ]
                                    ]
                                ]
                            ]

                            Grid.create [ 
                                Grid.isVisible isVisible.Current
                                Grid.columnDefinitions "*,*"
                                Grid.children [ 
                                    LabelingCol("Attack:", TextBox.create [
                                       TextBox.onTextChanged setAttack
                                    ],0)

                                    LabelingCol("Defense:", TextBox.create [
                                       TextBox.onTextChanged setDefence
                                       TextBox.isEnabled (not isLink.Current)
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
                                TextBox.acceptsReturn true
                                TextBox.textWrapping TextWrapping.Wrap
                                TextBox.onTextChanged(setDescription)
                            ])

                            Button.create [
                                Button.content "Save card"
                                Button.onClick saveCard
                                Button.clickMode ClickMode.Release
                            ]
                        ]
                    ])
                ]
            
            ]
        ] )



type MainWindow() =
    inherit SukiUI.Controls.SukiWindow()
    do
        base.Title <- "Yugioh Card Maker"
        base.MinWidth <- 500
        base.MinHeight <- 550
        base.Content <- Main.view ()


type App() =
    inherit Application()

    override this.Initialize() =
        // this.Styles.Add(FluentTheme())
        let theme = SukiUI.SukiTheme()
        theme.ThemeColor <- SukiUI.Enums.SukiColor.Blue
        this.Styles.Add(theme)
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
