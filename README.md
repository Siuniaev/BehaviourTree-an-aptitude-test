# Unity: BehaviourTree an aptitude test

Exercise:
Create a 3D Unity project with a player character, enemies, and a location using content of any format. Game mechanics - the player's character must move from the beginning to the end of the level with a positive HP pool in order to win. Character control is not required.

Create a character that has the following parameters: attack, defense, hp, speed (configured via config, you can expand the list if necessary). Character control is carried out with the help of AI (think about the implementation of FSM / BT). The character has the ability to attack/shoot from a distance if the target is in line of sight. When enemies approach, the hero must retreat and shoot back. Create multiple enemies with different characteristics. The parameters are the same as for the character. Enemies obey the general AI, they can only attack in close combat. The appearance of enemies on the map occurs in random places (spawn points), after which they move towards the character and seek to inflict damage on him in close combat. Upon reaching the end of the level, the enemies no longer appear, and the scene ends with the inscription “Win”. If the character dies, the inscription “Lose” is displayed.

The location must have various obstacles, walls, ups and downs. Walls should not be shot through and the characters should adequately respond to obstacles. Damage calculation should not be fixed, (you can make miss chance or crit chance or random from 0 to attack).
You need to add an enemy who can attack at a distance from a certain radius. The result of the replay should have a probability of being different.

The player's team must have several characters, they must have adequate logic of behavior. Heroes have skills (you can think of any) that are used when accumulating the power parameter, which grows for each successful hit (both incoming and outgoing).
After the end of the scene - run the next one with growing parameters of opponents.

Click on the [video link](https://vimeo.com/710727066) to see how the project works:
[![Video](https://i32.servimg.com/u/f32/11/33/69/31/aa10.png)](https://vimeo.com/710727066)
