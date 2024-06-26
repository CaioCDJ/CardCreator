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
