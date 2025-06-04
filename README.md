# Stack Overflow - Code Challenge #2: Secret messages in game boards

This repository contains the source code for my entry to the SO Code Challenge #2.

## Description

The challenge consists in encoding an 8+ characters message on the board state of the board game of our choice.

I chose to go with [Cluedo](https://cluedo.fandom.com/wiki/Cluedo_Board_Game) (also known as Clue in North America), as my significant other suggested.

There are [182 squares on a classic (1949) Cluedo board](https://cluedo.fandom.com/wiki/File:UK_Game_Board_(First_edition,_1949).jpg) (yes, I counted them by hand). There are [six distinguishable "suspects"](https://cluedo.fandom.com/wiki/Cluedo_1949#Suspects) (Red, Yellow, White, Green, Blue, and Purple). You can't have two paws on the same space, and for all intents and purposes of this challenge, we're going to ignore the rooms and cards players have in hand.

The permutations of each of these "suspects" pawns on the board will allow us to encode a message.

### Calculating the number of permutations

We can see our pawns and empty squares as distinguishable objects that can be permuted, of which we can calculate the number of possibilities.

[The formula is as follows](http://www.milefoot.com/math/discrete/counting/counting.htm): $\frac{n!}{{n_1!}\times{n_2!}\times{...}\times{n_k!}}$

With:
* $n$ the number of states each square can take. Here, we have 7 possible states for each square: ``Red``, ``Yellow``, ``White``, ``Green``, ``Blue``, ``Purple`` and ``Empty``.
* $n_k$ the number of occurences of the state $k$ in the board. Here, there is $1$ of each pawn, and $182 - 6 = 176$ empty spaces.

This leads us to $\frac{182!}{{1!}\times{1!}\times{1!}\times{1!}\times{1!}\times{1!}\times{176!}}=\frac{182!}{176!}=33,440,192,407,440$ possible permutations.

### Checking if this number of permutations is enough to encode 8+ characters

We will be using the following 49 characters alphabet: ``ABCDEFGHIJKLMNOPQRSTUVWXYZ !\"'()*+,-.0123456789:?``.

To calculate if $33,440,192,407,440$ possible permutations can encode 8+ of these characters, [we can use the following formula](https://stackoverflow.com/a/29847712/9399492) to check how many digits it takes to write the maximum permutation index: $1 + floor(log(N) / log(base))$

With:
* $N$ being the maximum permutation index we need to be able to store. Here, $33,440,192,407,440 - 1 = 33,440,192,407,439$.
* $base$ being the number of possible characters. Here, $49$.

This leads us to $1 + floor(log(33440192407439) / log(49)) = 9$

Meaning it takes 9 digits to encode the biggest number we can with our character set.

This leaves us with 8 characters we can safely store: one character has to be excluded, as it's incomplete. For example, in base 10, if you can count up to 30, even though it takes 2 digits to write "30", you won't be able to write all possible digits in the place of the "3", so only 1 digit is usable.

Do note, reducing our character set to just 26 letters would only give us one extra character to use. Considering one of the example messages has a ``!``, I opted for 49 characters, which is the maximum base for 8 characters with the limits we have here.

*Isn't it kind of crazy to go from 182 spaces to insane factorials to 33 trillions and get back to just 8?*

### Going from a message to a permutation number to an actual permutation on a board, and back

To be written.

Potential references:
- https://stackoverflow.com/a/1506337 & https://stackoverflow.com/a/24689277
- http://antoinecomeau.blogspot.com/2014/07/mapping-between-permutations-and.html

## Special thanks

- My significant other, for suggesting that I use a Cluedo board for this challenge
- [dCode](https://www.dcode.fr/), for being able to calculate insanely high [factorials](https://www.dcode.fr/factorial) and [divisions](https://www.dcode.fr/big-numbers-division) when [Google can't](https://www.google.com/search?q=(182!)%2F(176!))
- [Milefoot](http://www.milefoot.com/math/discrete/counting/counting.htm) for the permutations count formula
- [kratenko](https://stackoverflow.com/users/1358283/kratenko)['s answer](https://stackoverflow.com/a/29847712/9399492) for the number to digits count formula
- More to be added

## License

This whole repository, except for the parts indicated below, is licensed under [CC BY-SA 4.0](https://github.com/giroletm/SO-CC2/blob/master/LICENSE).

This software's icon is [the Stack Overflow "peak" icon](https://stackoverflow.design/product/foundation/icons/#peak), which [the challenges tab uses](https://stackoverflow.com/beta/challenges), and is licensed under [the MIT license](https://github.com/StackExchange/Stacks-Icons/blob/production/LICENSE.md).

