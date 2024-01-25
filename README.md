# Road Network Generator
 
Generates using L-Systems


## Highway
Axiom - [F][-F][--F]
Rule - F = F+F[F]


## City
Axiom - [FF][+FF][--FF][++FF]
Rule - F = F[+F][-FF]



Where 

F Forward
- Turn Left by Angle
+ Turn Right by Angle
[ Save Position
] Load Position

![Alt text](image.png)