# Unity-card-test
I made this as part of a test task

The test task was as follows:
Create UI for an "in-hand card" object for CCG-like game. Card consist of:
-Art + UI overlay;
-Title
-Description
-Attack icon + text value
-Health icon + text value
-Mana icon + text value

Load card art randomly from https://picsum.photos/ each time app starts.

Fill player's hand with 4-6 cards in a visually pleasing way and use the arc pattern
for displaying the cards. The number of cards should be determined randomly at the
start of the game.


Create an UI button at the center of the screen to randomly change one randomly selected
value (the range is from -2 to 9) of each one card sequentially, starting from the most
left card in the player's hand moving right and repeating the sequence after reaching
the most right card.

Bind Attack, Health and Mana properties to UI. Changing those values from code must be
reflected on the card's UI with counter animation.
(counting from the initial to the new value)

if some card's HP drop below 1 - remove this card from player's hand (don't forget to
reposition other cards, use tweens to make it smooth)

[Middle+] Player can drag a card and drop it on middle section of the table (use drop panel
of any size) Card moves back to player's hand if it's hasn't been dropped over the drop panel.
Cards shines when its being dragged.
