# Stack Overflow - Code Challenge #2: Secret messages in game boards

This repository contains the source code for my entry to the [Stack Overflow Code Challenge #2](https://stackoverflow.com/beta/challenges/79651567/code-challenge-2-secret-messages-in-game-boards).

## Short description

The challenge consists in encoding an 8+ characters message on the board state of the board game of our choice.

I chose to go with [Cluedo](https://cluedo.fandom.com/wiki/Cluedo_Board_Game) (also known as Clue in North America), as my significant other suggested.

## Usage

### Prerequisites

- Windows or [Wine](https://www.winehq.org/) (note that I haven't tested Wine compatiblity)
- [Visual Studio 2022](https://visualstudio.microsoft.com/en/thank-you-downloading-visual-studio/?sku=Community&channel=Release&version=VS2022)
- [.NET 8](https://dotnet.microsoft.com/fr-fr/download/dotnet/8.0)

### Compiling

- Open [``SO-CC2.sln``](https://github.com/giroletm/SO-CC2/blob/master/SO-CC2.sln) with Visual Studio 2022
- On top of the screen, change the building type to either:
    - "Debug", then press the green triangle button to start the project in debug mode
	- "Release", then press "Build" -> "Build SO-CC2" on the top bar, and you'll find a compiled executable in ``SO-CC2/bin/Release/net8.0-windows``.

### How does it work

Type the message you want to encode in the top right textbox to get the corresponding Cluedo board.

Alternatively, drag-and-drop pawns accross the board to get the corresponding message. [WIP]

## Explanation

There are [182 squares on a classic (1949) Cluedo board](https://cluedo.fandom.com/wiki/File:UK_Game_Board_(First_edition,_1949).jpg) (yes, I counted them by hand). There are [six distinguishable "suspects"](https://cluedo.fandom.com/wiki/Cluedo_1949#Suspects) (Red, Yellow, White, Green, Blue, and Purple). You can't have two paws on the same space, and for all intents and purposes of this challenge, we're going to ignore the rooms, and the cards players have in hand.

The permutations of each of these "suspects" pawns on the board will allow us to encode a message.

### Calculating the number of permutations

We can see our pawns and empty squares as distinguishable objects that can be permuted, of which we can calculate the number of possibilities.

[The formula is as follows](http://www.milefoot.com/math/discrete/counting/counting.htm): $\frac{n!}{{n_1!}\times{n_2!}\times{...}\times{n_k!}}$

With:
* $n$ the number of states each square can take. Here, we have 7 possible states for each square: ``Red``, ``Yellow``, ``White``, ``Green``, ``Blue``, ``Purple`` and ``Empty``.
* $n_k$ the number of occurences of the state $k$ in the board. Here, there is $1$ of each pawn, and $182 - 6 = 176$ empty spaces.

This leads us to $\frac{182!}{{1!}\times{1!}\times{1!}\times{1!}\times{1!}\times{1!}\times{176!}}=\frac{182!}{176!}=33,440,192,407,440$ possible permutations.

### Going from a message to a permutation number, and back

It's quite easy, really, just consider the message as a number in the base composed of the character set you've chosen.

We will be using the following 49 character set: `` ABCDEFGHIJKLMNOPQRSTUVWXYZ!\"'()*+,-.0123456789:?``.

This means we must simply parse a message as a base 49 number to get its numerical value, which can also be done in reverse to encode messages.

#### Encoding and decoding numbers to and from any base

Encoding can be done using [a succession of Euclidean divisions by the base number](https://en.wikipedia.org/wiki/Positional_notation#Base_conversion). The quotient is the index in the character set for the first rightmost digit, and you can repeat this operation on the remainder to calculate the second rightmost digit, third, etc until quotient is 0.

Decoding can be done using [Horner's method](https://en.wikipedia.org/wiki/Horner%27s_method), by multiplying each digit's index in the character set by the base number to the power of its position within the number.

#### Checking if the total number of permutations is enough to encode 8+ characters

To calculate if $33,440,192,407,440$ possible permutations can encode 8+ of these characters, [we can use the following formula](https://stackoverflow.com/a/29847712/9399492) to check how many digits it takes to write the maximum permutation index: $1 + floor(log(N) / log(base))$

With:
* $N$ being the maximum permutation index we need to be able to store. Here, $33,440,192,407,440 - 1 = 33,440,192,407,439$.
* $base$ being the number of possible characters. Here, $49$.

This leads us to $1 + floor(log(33440192407439) / log(49)) = 9$

Meaning it takes 9 digits to encode the biggest number we can with our character set.

This leaves us with 8 characters we can safely store: one character has to be excluded, as it's incomplete. For example, in base 10, if you can count up to 30, even though it takes 2 digits to write "30", you won't be able to write all possible digits in the place of the "3", so only 1 digit is usable.

Do note, reducing our character set to just 26 letters would only give us one extra character to use. Considering one of the example messages has a ``!``, I opted for 49 characters, which is the maximum base for 8 characters with the limits we have here.

*Isn't it kind of crazy to go from 182 spaces to insane factorials to 33 trillions and get back to just 8?*

### Going from a permutation number to an actual permutation on a board, and back

#### Representing the board state as a permutation

No need for anything fancy: just have a string with as many spaces are there are empty spaces, and a unique character for each pawn. Their arrangement will define on which square is each pawn placed.

For example, if I have the following string: ``"12345 6                                                                                                                                                                               "`` (notice the 176 spaces in there)

Then it means that pawn ``1`` is on the first square, ``2`` on the second, ``3`` on the third, ``4`` on the fourth, ``5`` on the fifth, and ``6`` on the seventh, since I put a space before it.

You can then shuffle the characters of this string the way you want, and you'll get new pawn positions, **always in a legal state**, you will never have two pawns overlapping each other or more than the 6 base pawns.

#### Convertion between a permutation number and a permutation of the board state

There are various ways to perform this. In fact, I myself [opened a question about it on Stack Overflow](https://stackoverflow.com/q/79652400/9399492), which I encourage you to check out if you're interested into learning more!

To get a permutation number from an actual permutation on a board, I chose to base my implementation on [Vepir's](https://math.stackexchange.com/a/3803239), which was itself based on [Shubham Nanda's explanation](https://math.stackexchange.com/a/1797885):

> $[\frac{n!}{{n_1!}\times{n_2!}\times{...}\times{n_k!}}]$ is the multinomial coefficient. Now we can use this to compute the rank of a given permutation as follows:
> 
> Consider the first character (leftmost). say it was the r^th one in the sorted order of characters.
> 
> Now if you replace the first character by any of the 1,2,3,..,(r-1)^th character and consider all possible permutations, each of these permutations will precede the given permutation. The total number can be computed using the above formula.
> 
> Once you compute the number for the first character, fix the first character, and repeat the same with the second character and so on.

For the opposite process, I based my implementation on [Bartosz Marcinkowski's answer](https://stackoverflow.com/a/24508736/9399492), which recursively calls itself to calculate the character on each index, one by one, and appending the results.

To keep the same reference between the two, I consider the base permutation as the Unicode value-sorted board state string, which is ``"                                                                                                                                                                                123456"``.

Then, all that's left to do is to pass this enormous string as a base along with a permutation index/rank/number to get the board state that corresponds to it using that second algorithm!

The opposite can also be done by passing the permutated board state into the first algorithm.

### Conclusion

And voilà, we got ourselves our Cluedo message encoder. Now that we've all grown up emotionally using some nice, juicy maths, I wonder what's to come with SO code challenges.

I've learnt a lot of things, this challenge was for sure much more interesting than the [previous one](https://stackoverflow.com/beta/challenges/79640866/complete-code-challenge-1-implement-a-text-to-baby-talk-translator)!

## Special thanks

- My significant other, for suggesting that I use a Cluedo board for this challenge
- [dCode](https://www.dcode.fr/), for being able to calculate insanely high [factorials](https://www.dcode.fr/factorial) and [divisions](https://www.dcode.fr/big-numbers-division) when [Google can't](https://www.google.com/search?q=(182!)%2F(176!))
- [Milefoot](http://www.milefoot.com/math/discrete/counting/counting.htm) for the permutations count formula
- [kratenko](https://stackoverflow.com/users/1358283/kratenko)['s answer](https://stackoverflow.com/a/29847712/9399492) for the number to digits count formula
- [Vepir](https://math.stackexchange.com/users/318073/vepir)['s answer](https://math.stackexchange.com/a/3803239) and [Shubham Nanda](https://math.stackexchange.com/users/318202/shubham-nanda)['s answer](https://math.stackexchange.com/a/1797885) for the permutation to index algorithm
- [Bartosz Marcinkowski](https://stackoverflow.com/users/2307066/bartosz-marcinkowski)['s answer](https://stackoverflow.com/a/24508736/9399492) for the index to permutation algorithm
- [All of the participants to my SO question on the matter](https://stackoverflow.com/q/79652400/9399492), who not only gave me valuable answers, but also helped improve the question
- [Wikipedia](https://www.wikipedia.org/)

## License

This whole repository, except for the parts indicated below, is licensed under [CC BY-SA 4.0](https://github.com/giroletm/SO-CC2/blob/master/LICENSE).

This software's icon is [the Stack Overflow "peak" icon](https://stackoverflow.design/product/foundation/icons/#peak), which [the challenges tab uses](https://stackoverflow.com/beta/challenges), and is licensed under [the MIT license](https://github.com/StackExchange/Stacks-Icons/blob/production/LICENSE.md).

