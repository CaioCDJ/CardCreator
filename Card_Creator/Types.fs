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

    type Card =
        { name: string
          description: string
          image: string
          cardType: CardType
          level: int
          atk: int
          def: int
          attribute: attributes
        }

    // Monsters Types

    type MonsterType =
        | Aqua
        | Beast
        | BeastWarrior
        | CreatorGod
        | Cyberse
        | Dinosaur
        | Dragon
        | Fairy
        | Fiend
        | Fish
        | Insect
        | Machine
        | Plant
        | Psychic
        | Pyro
        | Reptile
        | Rock
        | SeaSerpent
        | Spellcaster
        | Thunder
        | Warrior
        | WingedBeast
        | Wyrm
        | Zombie

    type Monster =
        { atk: int
          def: int
          level: int
          atribute: attributes
          Type: MonsterType
         }

    // spells and traps
    
