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

    let addTitle ((card, image): Card * Image) : Card * Image =
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

    let addLevels ((card, image): Card * Image) : Card * Image =

        let level =
            Image.Load(
                match card.cardType with
                | CardType.Xyz -> "./assets/Rank.png"
                | _ -> "./assets/Level.png"
            )


        if card.cardType = CardType.Xyz then
            for i in 1 .. card.monster.Value.level do
                let pointW = if i = 1 then 148 else (148 - (level.Width - (95 * i)))

                image.Mutate(fun x -> x.DrawImage(level, Point(pointW, 243), 1f) |> ignore)

        else
            for i in 1 .. card.monster.Value.level do
                let pointW = if i = 1 then 1169 else (1169 + (level.Width - (95 * i)))

                image.Mutate(fun x -> x.DrawImage(level, Point(pointW, 243), 1f) |> ignore)

        (card, image)


    let addType ((card, image): Card * Image) : Card * Image =

        let textType = $"[{card.monster.Value.Type}]"

        let font = SystemFonts.CreateFont("Arial", 45f, FontStyle.Bold)
        let optionsType = RichTextOptions(font)

        optionsType.Origin <- PointF(107f, 1528f)
        optionsType.WordBreaking <- WordBreaking.Standard
        optionsType.WrappingLength <- 1165f
        optionsType.TextJustification <- TextJustification.InterWord
        optionsType.VerticalAlignment <- VerticalAlignment.Top
        optionsType.HorizontalAlignment <- HorizontalAlignment.Left

        let brush = Brushes.Solid(Color.Black)

        image.Mutate(fun x -> x.DrawText(optionsType, textType, brush) |> ignore)

        (card, image)


    let addAttribute ((card, image): Card * Image) : Card * Image =

        let attrImg = Image.Load(attributeAssets.[card.monster.Value.atribute])

        attrImg.Mutate(fun x -> x.Resize(131, 131) |> ignore)

        image.Mutate(fun x -> x.DrawImage(attrImg, Point(1162, 88), 1f) |> ignore)

        (card, image)

    let addBattleAttr ((card, image): Card * Image) : Card * Image =

        let font = SystemFonts.CreateFont("Arial", 52f, FontStyle.Bold)

        let optionsAttribute = RichTextOptions(font)

        optionsAttribute.Origin <- PointF(884f, 1849f)
        optionsAttribute.WordBreaking <- WordBreaking.Standard
        optionsAttribute.WrappingLength <- 1165f
        optionsAttribute.TextJustification <- TextJustification.InterWord
        optionsAttribute.VerticalAlignment <- VerticalAlignment.Top
        optionsAttribute.HorizontalAlignment <- HorizontalAlignment.Left

        let brush = Brushes.Solid(Color.Black)

        image.Mutate(fun x ->
            x.DrawText(optionsAttribute, card.monster.Value.attack.ToString(), brush)
            |> ignore)

        optionsAttribute.Origin <- PointF(1159f, 1849f)

        if card.cardType <> CardType.Link then
            image.Mutate(fun x ->
                x.DrawText(optionsAttribute, card.monster.Value.defence.ToString(), brush)
                |> ignore)

        (card, image)

    let addArrows ((card, image): Card * Image) : Card * Image =

        if card.monster.Value.linkArrows.IsSome then

            let arrowD = Image.Load(arrowAsset true)
            let arrow = Image.Load(arrowAsset false)

            // straight
            if card.monster.Value.linkArrows.Value.top then
                let arrowM  = Image.Load(arrowAsset false)
                image.Mutate(fun x -> x.DrawImage(arrowM, Point(559, 299), 1f) |> ignore)

            if card.monster.Value.linkArrows.Value.bottom then
                let arrowM  = Image.Load(arrowAsset false)
                arrowM.Mutate(fun x -> x.Rotate(180f) |> ignore)
                image.Mutate(fun x -> x.DrawImage(arrowM, Point(559, 1425), 1f) |> ignore)

            if card.monster.Value.linkArrows.Value.left then
                let arrowM  = Image.Load(arrowAsset false)
                arrowM.Mutate(fun x -> x.Rotate(270f) |> ignore)
                image.Mutate(fun x -> x.DrawImage(arrowM, Point(95, 763), 1f) |> ignore)

            if card.monster.Value.linkArrows.Value.right then
                let arrowM  = Image.Load(arrowAsset false)
                arrowM.Mutate(fun x -> x.Rotate(90f) |> ignore)
                image.Mutate(fun x -> x.DrawImage(arrowM, Point(1223, 763), 1f) |> ignore)

            // diagonal
            if card.monster.Value.linkArrows.Value.topLeft then
                let arrowM  = Image.Load(arrowAsset true)
                image.Mutate(fun x -> x.DrawImage(arrowD, Point(111, 317), 1f) |> ignore)

            if card.monster.Value.linkArrows.Value.topRight then
                let arrowM  = Image.Load(arrowAsset true)
                arrowM.Mutate(fun x -> x.Rotate(90f) |> ignore)
                image.Mutate(fun x -> x.DrawImage(arrowM, Point(1151, 317), 1f) |> ignore)

            if card.monster.Value.linkArrows.Value.bottomRight then
                let arrowM  = Image.Load(arrowAsset true)
                arrowM.Mutate(fun x -> x.Rotate(180f) |> ignore)
                image.Mutate(fun x -> x.DrawImage(arrowM, Point(1139, 1358), 1f) |> ignore)

            if card.monster.Value.linkArrows.Value.bottomLeft then
                let arrowM  = Image.Load(arrowAsset true)
                arrowM.Mutate(fun x -> x.Rotate(270f) |> ignore)
                image.Mutate(fun x -> x.DrawImage(arrowM, Point(111, 1358), 1f) |> ignore)

            (card, image)
        else
            (card, image)

    let addLinkRate ((card, image): Card * Image) : Card * Image =
        let mutable rate = 0

        if card.monster.Value.linkArrows.IsSome then

            if card.monster.Value.linkArrows.Value.top then
                rate <- rate + 1

            if card.monster.Value.linkArrows.Value.bottom then
                rate <- rate + 1

            if card.monster.Value.linkArrows.Value.left then
                rate <- rate + 1

            if card.monster.Value.linkArrows.Value.right then
                rate <- rate + 1

            if card.monster.Value.linkArrows.Value.topLeft then
                rate <- rate + 1

            if card.monster.Value.linkArrows.Value.topRight then
                rate <- rate + 1

            if card.monster.Value.linkArrows.Value.bottomRight then
                rate <- rate + 1

            if card.monster.Value.linkArrows.Value.bottomLeft then
                rate <- rate + 1

            let font = SystemFonts.CreateFont("Arial", 75f, FontStyle.Regular)

            let optionsName = RichTextOptions(font)
            optionsName.Origin <- PointF(1199f, 1839f)
            optionsName.TabWidth <- 0.0f
            optionsName.WordBreaking <- WordBreaking.Standard
            optionsName.WrappingLength <- 1048f
            optionsName.HorizontalAlignment <- HorizontalAlignment.Left

            let brush = Brushes.Solid(Color.Black)

            image.Mutate(fun x -> x.DrawText(optionsName, "-"+ rate.ToString(), brush) |> ignore)

            (card, image)
        else
            (card, image)


    let handle (cd: Card option) =

        if not (cd.IsNone) then
            
            let imageTemplate = Image.Load(cardTemplates.[cd.Value.cardType])

            addTitle (cd.Value, imageTemplate)
            |> addDescription
            |> addImage
            |> (fun (card, image) ->
                if card.cardType <> CardType.Spell && card.cardType <> CardType.Trap then
                    addType (card, image)
                    |> addBattleAttr
                    |> addAttribute
                    |> (fun (card, image) ->
                        if card.cardType = CardType.Link then
                            addArrows (card, image) |> addLinkRate
                        else
                            addLevels (card, image))
                else
                    (card, image))
            |> (fun (card, image) -> image.Save($"{cd.Value.name}.png"))
            
            printf $"{cd.Value.name}.png was saved!"
