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
        | Pendulum
        | Spell
        | Trap

    type attributes =
        | Fire
        | Water
        | Wind
        | Earth
        | Light
        | Dark

    type Monster =
        { attack: int
          defence: int
          level: int
          atribute: attributes
          Type: string }


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
              CardType.Trap, "avares://Card_Creator/assets/cardTemplates/Trap.jpeg" ]

    let toEnum (x: string) =
        match x with
        | "Normal" -> CardType.Normal
        | "Effect" -> CardType.Effect
        | "Fusion" -> CardType.Fusion
        | "Synchro" -> CardType.Synchro
        | "Link" -> CardType.Link
        | "Ritual" -> CardType.Ritual
        | "Xyz" -> CardType.Xyz
        | "Pendulum" -> CardType.Pendulum
        | "Spell" -> CardType.Spell
        | "Trap" -> CardType.Trap
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
