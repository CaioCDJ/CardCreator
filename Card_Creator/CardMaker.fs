namespace Card_Creator

open Card_Creator.CardTypes
open SixLabors.ImageSharp
open SixLabors
open SixLabors.ImageSharp.Processing
open SixLabors.ImageSharp.Drawing
open SixLabors.Fonts
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Drawing.Processing
open System

module CardMaker =

    let cardTemplates =
        Map
            [ CardType.Normal, "./assets/cardTemplates/Normal.jpeg"
              CardType.Effect, "./assets/cardTemplates/Effect.jpeg"
              CardType.Fusion, "./assets/cardTemplates/Fusion.jpeg"
              CardType.Synchro, "./assets/cardTemplates/synchro.jpeg"
              CardType.Link, "./assets/cardTemplates/Link.jpeg"
              CardType.Ritual, "./assets/cardTemplates/Ritual.jpeg"
              CardType.Xyz, "./assets/cardTemplates/Xyz.jpeg"
              CardType.Spell, "./assets/cardTemplates/Spell.jpeg"
              CardType.Trap, "./assets/cardTemplates/Trap.jpeg" ]

    let attributeAssets =
        Map
            [ attributes.Dark, "./assets/attributes/DARK.png"
              attributes.Earth, "./assets/attributes/EARTH.png"
              attributes.Fire, "./assets/attributes/FIRE.png"
              attributes.Light, "./assets/attributes/LIGHT.png"
              attributes.Water, "./assets/attributes/WATER.png"
              attributes.Wind, "./assets/attributes/WIND.png" ]

    // bigger names for you
    let normalizeName = ""

    let genericInfo (image: Image) : Image =
        //Name and Description type
        
        let font = SystemFonts.CreateFont("Arial", 80f, FontStyle.Regular)

        let optionsName = RichTextOptions(font)
        optionsName.Origin <- PointF(106f, 122.0f)
        optionsName.TabWidth <- 0.0f
        optionsName.WordBreaking <- WordBreaking.Standard
        optionsName.WrappingLength <- 1048f
        optionsName.HorizontalAlignment <- HorizontalAlignment.Left
        
        let brush = Brushes.Solid(Color.White)

        let pen = Pens.Solid(Color.Gray, 1.0f)

        let text = "Dog of Greed"

        image.Mutate(fun x -> x.DrawText(optionsName, text, brush, pen)|>ignore)

        let optionsDescription = RichTextOptions(font)
        optionsDescription.Origin <- PointF(35.0f, 33.0f)
        optionsDescription.TabWidth <- 0.0f
        optionsDescription.WordBreaking <- WordBreaking.Standard
        optionsDescription.WrappingLength <- 1048f
        optionsDescription.HorizontalAlignment <- HorizontalAlignment.Left
        
        image


    let addDescription (image: Image) =

        let font = SystemFonts.CreateFont("Arial", 37f, FontStyle.Regular)

        let optionsDescription = RichTextOptions(font)
        optionsDescription.Origin <-  PointF(109.0f, 1530.0f)
        optionsDescription.WordBreaking <- WordBreaking.Standard
        optionsDescription.WrappingLength <- 1165f
        optionsDescription.TextJustification <- TextJustification.InterWord
        optionsDescription.VerticalAlignment <- VerticalAlignment.Top
        optionsDescription.HorizontalAlignment <- HorizontalAlignment.Left

        let txt = "Lorem ipsum dolor sit amet, officia excepteur ex fugiat reprehenderit enim labore culpa sint ad nisi Lorem pariatur mollit ex esse exercitation amet. Nisi anim cupidatat excepteur officia. Reprehenderit nostrud nostrud ipsum Lorem est aliquip amet voluptate voluptate dolor minim nulla est proident. Nostrud officia pariatur ut officia. Sit irure elit esse ea nulla sunt ex occaecat reprehenderit commodo officia dolor Lorem duis laboris cupidatat officia voluptate. Culpa proident adipisicing id nulla nisi laboris ex in Lorem sunt duis officia eiusmod. Aliqua reprehenderit commodo ex non excepteur duis sunt velit enim. Voluptate laboris sint cupidatat ullamco ut ea consectetur et est culpa et culpa duis."
        let brush = Brushes.Solid(Color.Black)
        
        image.Mutate(fun x -> x.DrawText(optionsDescription, txt, brush)|>ignore)
        
        image


    let addImage (image: Image) = 
        
        let otherImage = Image.Load("./assets/oliver.jpeg")

        let location = Point(168,372)

        otherImage.Mutate(fun x-> x.Resize(1051,1051) |> ignore)
        
        image.Mutate(fun x -> x.DrawImage(otherImage, location, 1f)|>ignore)
        
        image

    let putLevels (image: Image) : Image = image
    
    let addAttribute (image: Image) = ""
    
    let monsterInfo (image: Image) = ""

    let handle =
        let imageTemplate = Image.Load(cardTemplates.[CardType.Spell])
        genericInfo imageTemplate   
        |> addDescription
        |> addImage
        |> putLevels 
        |> (fun x -> x.Save "example.png")
         
        0
