namespace Card_Creator

open Card_Creator.CardTypes

module CardMaker =

    let getImageAsset (card: CardType) : string =
        match card with
        | CardType.Normal -> "assets/normal.png"
        | CardType.Effect -> "assets/effect.png"
        | CardType.Fusion -> "assets/fusion.png"
        | CardType.Synchro -> "assets/synchro.png"
        | CardType.Link -> "assets/link.png"
        | CardType.Ritual -> "assets/ritual.png"
        | CardType.Xyz -> "assets/xyz.png"
        | CardType.Pendulum -> "assets/pendulum.png"
        | CardType.Spell -> "assets/magic.png"
        | CardType.Trap -> "assets/trap.png"
        | _ -> ""

    let genericInfo = ""

    let putLevels = ""
     
    let addAttribute = ""

    let monsterInfo = ""

    let handle =  ""

