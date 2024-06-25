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

    let genericInfo ((card, image): Card * Image) : Card * Image =
        //Name and Description type

        let font = SystemFonts.CreateFont("Arial", 80f, FontStyle.Regular)

        let optionsName = RichTextOptions(font)
        optionsName.Origin <- PointF(106f, 122.0f)
        optionsName.TabWidth <- 0.0f
        optionsName.WordBreaking <- WordBreaking.Standard
        optionsName.WrappingLength <- 1048f
        optionsName.HorizontalAlignment <- HorizontalAlignment.Left

        let brush =
            Brushes.Solid(
                if
                    card.cardType = CardType.Xyz
                    || card.cardType = CardType.Spell
                    || card.cardType = CardType.Trap
                then
                    Color.WhiteSmoke
                else
                    Color.Black
            )

        let pen = Pens.Solid(Color.Gray, 1.0f)

        image.Mutate(fun x -> x.DrawText(optionsName, card.name, brush, pen) |> ignore)

        (card, image)


    let addDescription ((card, image): Card * Image) : Card * Image =

        let font = SystemFonts.CreateFont("Arial", 37f, FontStyle.Regular)

        let optionsDescription = RichTextOptions(font)

        optionsDescription.Origin <-
            PointF(
                109.0f,
                if card.cardType = CardType.Trap || card.cardType = CardType.Spell then
                    1530.0f
                else
                    1580.0f
            )

        optionsDescription.WordBreaking <- WordBreaking.Standard
        optionsDescription.WrappingLength <- 1165f
        optionsDescription.TextJustification <- TextJustification.InterWord
        optionsDescription.VerticalAlignment <- VerticalAlignment.Top
        optionsDescription.HorizontalAlignment <- HorizontalAlignment.Left


        let brush = Brushes.Solid(Color.Black)

        image.Mutate(fun x -> x.DrawText(optionsDescription, card.description, brush) |> ignore)

        (card, image)

    let addImage ((card, image): Card * Image) : Card * Image =

        let otherImage = Image.Load(card.image)

        let location = Point(168, 372)

        otherImage.Mutate(fun x -> x.Resize(1051, 1051) |> ignore)

        image.Mutate(fun x -> x.DrawImage(otherImage, location, 1f) |> ignore)

        (card, image)

    let addType ((card, image): Card * Image) : Card * Image = (card, image)

    let addLevels ((card, image): Card * Image) : Card * Image =


        let level =
            Image.Load(
                match card.cardType with
                | CardType.Xyz -> "./assets/Rank.png"
                | _ -> "./assets/Level.png"
            )


        if card.cardType = CardType.Xyz then
            for i in 1 .. card.level do
                let pointW = if i = 1 then 148 else (148 - (level.Width - (95 * i)))

                image.Mutate(fun x -> x.DrawImage(level, Point(pointW, 243), 1f) |> ignore)

        else
            for i in 1 .. card.level do
                let pointW = if i = 1 then 1169 else (1169 + (level.Width - (95 * i)))

                image.Mutate(fun x -> x.DrawImage(level, Point(pointW, 243), 1f) |> ignore)


        (card, image)


    let addAttribute ((card, image): Card * Image) : Card * Image = (card, image)

    let addBattleAttr ((card, image): Card * Image) : Card * Image = (card, image)

    let handle =

        let card: Card =
            { name = "Oliver, the boxer"
              description =
                "Lorem ipsum dolor sit amet, officia excepteur ex fugiat reprehenderit enim labore culpa sint ad nisi Lorem pariatur mollit ex esse exercitation amet. Nisi anim cupidatat excepteur officia. Reprehenderit nostrud nostrud ipsum Lorem est aliquip amet voluptate voluptate dolor minim nulla est proident. Nostrud officia pariatur ut officia. Sit irure elit esse ea nulla sunt ex occaecat reprehenderit commodo officia dolor Lorem duis laboris cupidatat officia voluptate. Culpa proident "
              attribute = attributes.Dark
              level = 8
              cardType = CardType.Effect
              image = "./assets/oliver.jpeg"
              atk = 3200
              def = 0 }

        let imageTemplate = Image.Load(cardTemplates.[card.cardType])

        genericInfo (card, imageTemplate)
        |> addDescription
        |> addImage
        |> (fun (card, image) ->
            if card.cardType <> CardType.Spell && card.cardType <> CardType.Trap then
                addLevels (card, image) |> addType
            else
                (card, image))
        |> (fun (card, image) -> image.Save("example.png"))

        0
