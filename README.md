# Stack Overflow - Code Challenge #2: Secret messages in game boards

This repository contains the source code for my entry to the SO Code Challenge #2.

## Description

The challenge consists in encoding an 8+ characters message on the board state of the board game of our choice.

I chose to go with [Cluedo](https://cluedo.fandom.com/wiki/Cluedo_Board_Game) (also known as Clue in North America), as my significant other suggested.

There are [183 squares on a classic (1949) Cluedo board](https://cluedo.fandom.com/wiki/File:UK_Game_Board_(First_edition,_1949).jpg) (yes, I counted them by hand). There are [six distinguishable "suspects"](https://cluedo.fandom.com/wiki/Cluedo_1949#Suspects) (Red, Yellow, White, Green, Blue, and Purple). You can't have two paws on the same space, and for all intents and purposes of this challenge, we're going to ignore the rooms and cards players have in hand.

The permutations of each of these "suspects" pawns on the board will allow us to encode a message.

### Calculating the number of permutations

We can see our pawns and empty squares as distinguishable objects that can be permuted, of which we can calculate the number of possibilities.

[The formula is as follows](http://www.milefoot.com/math/discrete/counting/counting.htm): $\frac{n!}{{n_1!}\times{n_2!}\times{...}\times{n_k!}}$

With:

* $n$ the number of states each square can take. Here, we have 7 possible states for each square: ``Red``, ``Yellow``, ``White``, ``Green``, ``Blue``, ``Purple`` and ``Empty``.

* $n_k$ the number of occurences of the state $k$ in the board. Here, there is $1$ of each pawn, and $183 - 6 = 177$ empty spaces.

This leads us to $\frac{183!}{{1!}\times{1!}\times{1!}\times{1!}\times{1!}\times{1!}\times{177!}}=\frac{183!}{177!}=34,573,758,251,760$ possible permutations.

### Checking if this number of permutations is enough to encode 8+ characters

We will be using the following 53 characters alphabet: ``ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 !"%&'()*+,-.:;?``. Alternatively, one could substitute it with ``ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ``, if you want case-sensitive messages.

To calculate if $34,573,758,251,760$ possible permutations can encode 8+ of these characters, [we can use the following formula](https://stackoverflow.com/a/29847712/9399492): $1 + floor(log(N) / log(base))$

With:

* $N$ being the number of permutations. Here, $34,573,758,251,760$.

* $base$ being the number of possible characters. Here, $53$.

This leads us to $1 + floor(log(34573758251760) / log(53)) = 8$

Meaning we can encode, at best, 8 characters with all of these permutations.

*Isn't it kind of crazy to go from 183 spaces to insane factorials to 34 trillions and get back to just 8?*

### Going from a message to a permutation number to an actual permutation on a board, and back

To be written.

Potential references:

- https://stackoverflow.com/a/1506337 & https://stackoverflow.com/a/24689277

- http://antoinecomeau.blogspot.com/2014/07/mapping-between-permutations-and.html

## Special thanks

- My significant other, for suggesting that I use a Cluedo board for this challenge

- [dCode](https://www.dcode.fr/), for being able to calculate insanely high [factorials](https://www.dcode.fr/factorial) and [divisions](https://www.dcode.fr/big-numbers-division) when [Google can't](https://www.google.com/search?q=(183!)%2F(177!))

- [Milefoot](http://www.milefoot.com/math/discrete/counting/counting.htm) for the permutations count formula

- [kratenko](https://stackoverflow.com/users/1358283/kratenko)['s answer](https://stackoverflow.com/a/29847712/9399492) for the number to digits count formula

- More to be added

## License

This whole repository is licensed under [CC BY-SA 4.0](https://github.com/giroletm/SO-CC2/blob/master/LICENSE).