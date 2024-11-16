namespace Card_Creator

module CardTypes =

    // Generics
    type CardType =
        | Normal
        | Effect
        | Fusion
        | Synchro
        | Link
        | Ritual
        | Xyz
        | Pendulum_Normal
        | Pendulum_Effect
        | Pendulum_Fusion
        | Pendulum_Synchro
        | Pendulum_Xyz
        | Spell
        | Trap

    type attributes =
        | Fire
        | Water
        | Wind
        | Earth
        | Light
        | Dark

    type LinkArrows =
        { left: bool
          right: bool
          top: bool
          bottom: bool
          topLeft: bool
          topRight: bool
          bottomLeft: bool
          bottomRight: bool }

    type Monster =
        { attack: int
          defence: int
          level: int
          atribute: attributes
          Type: string
          linkArrows: LinkArrows option }


    type Card =
        { name: string
          description: string
          image: string
          cardType: CardType
          monster: Monster option }


    let TemplateAssets =
        Map
            [ CardType.Normal, "avares://Card_Creator/assets/cardTemplates/Normal.jpeg"
              CardType.Effect, "avares://Card_Creator/assets/cardTemplates/Effect.jpeg"
              CardType.Fusion, "avares://Card_Creator/assets/cardTemplates/Fusion.jpeg"
              CardType.Synchro, "avares://Card_Creator/assets/cardTemplates/Synchro.jpeg"
              CardType.Link, "avares://Card_Creator/assets/cardTemplates/Link.jpeg"
              CardType.Ritual, "avares://Card_Creator/assets/cardTemplates/Ritual.jpeg"
              CardType.Xyz, "avares://Card_Creator/assets/cardTemplates/Xyz.jpeg"
              CardType.Spell, "avares://Card_Creator/assets/cardTemplates/Spell.jpeg"
              CardType.Trap, "avares://Card_Creator/assets/cardTemplates/Trap.jpeg" 
              CardType.Pendulum_Normal, "avares://Card_Creator/assets/cardTemplates/Pendulum_Normal.jpeg"
              CardType.Pendulum_Effect, "avares://Card_Creator/assets/cardTemplates/Pendulum_Effect.jpeg"
              CardType.Pendulum_Fusion, "avares://Card_Creator/assets/cardTemplates/Pendulum_Fusion.jpeg"
              CardType.Pendulum_Synchro, "avares://Card_Creator/assets/cardTemplates/Pendulum_Synchro.jpeg"
              CardType.Pendulum_Xyz, "avares://Card_Creator/assets/cardTemplates/Pendulum_Xyz.jpeg"
              ]

    let toEnum (x: string) =
        match x with
        | "Normal" -> CardType.Normal
        | "Effect" -> CardType.Effect
        | "Fusion" -> CardType.Fusion
        | "Synchro" -> CardType.Synchro
        | "Link" -> CardType.Link
        | "Ritual" -> CardType.Ritual
        | "Xyz" -> CardType.Xyz
        | "Spell" -> CardType.Spell
        | "Trap" -> CardType.Trap
        | "Pendulum_Normal" -> CardType.Pendulum_Normal
        | "Pendulum_Effect" -> CardType.Pendulum_Effect
        | "Pendulum_Fusion" -> CardType.Pendulum_Fusion
        | "Pendulum_Synchro" -> CardType.Pendulum_Synchro
        | "Pendulum_Xyz" -> CardType.Pendulum_Xyz
        | _ -> CardType.Normal

    let attributesAssets =
        Map
            [ attributes.Fire, "avares://Card_Creator/assets/attributes/FIRE.png"
              attributes.Water, "avares://Card_Creator/assets/attributes/WATER.png"
              attributes.Wind, "avares://Card_Creator/assets/attributes/WIND.png"
              attributes.Earth, "avares://Card_Creator/assets/attributes/EARTH.png"
              attributes.Light, "avares://Card_Creator/assets/attributes/LIGHT.png"
              attributes.Dark, "avares://Card_Creator/assets/attributes/DARK.png" ]

    let attrToEnum (attr: string) =
        match attr with
        | "Fire" -> attributes.Fire
        | "Water" -> attributes.Water
        | "Wind" -> attributes.Wind
        | "Earth" -> attributes.Earth
        | "Light" -> attributes.Light
        | "Dark" -> attributes.Dark
        | _ -> attributes.Fire

    let arrowAsset (diagonal: bool) : string =
        if diagonal then
            "./assets/arrowDiagonal.png"
        else
            "./assets/arrow.png"
