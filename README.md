# UPS-sim 

*AUTHORS: Matthew Low, Donald Lee*

# OVERVIEW 
A delivery service game where the player delivers goods across a map in a set amount of time, balancing between speed of delivery and quality of service to earn money

# ENGINE 
Unity, C#

# PLATFORM (TBD) 
Windows, WebGL 

# FEATURES (! indicates TBD):
	- Topdown 2D game
	- One giant map
	- Camera tracks vehicle/ Fixed camera orientation !
	- Day-by-day missions
		- User picks up random set of goods (different levels of fragility) at starting location
		- Different dropoff locations, user determines order in which to deliver
		- Upon approaching dropoff location:
			- Location has dropoff circle around it
			- Within the circle, a trajectory cone appears around the vehicle where the user can launch the appropriate good(s)
			- The faster the user's current speed, the larger the cone's angle
			- Good will be launched at a random location within the cone
			- Force mechanic will determine magnitude of radius of cone (through spacebar press and release)
			- If good lands outside cone, delivery failed (quality of service = 0)
			- The more force used, the lower the integrity of the good will be upon impact (quality of service = 0)
		- The lower the quality of service for any delivery, the less money received at the end of the mission
	- Vehicle takes damage upon collisions
	- Vehicle damage carries over to the next day
	- Money can be used to repair vehicle
	- Game over when vehicle totalled