## Player Merge Service
* This service merges the player data from the BHQ Stats service and the Player service.
  * Gets the "new" players from the BHQ Stats service.
  * Gets the "old" players from the Player service.
  * Merges the two lists of players.
  * Posts the merged list of players back to the Player service.

---
### Endpoints:
* `POST api/player/merge` - Executes the merge logic stated above.

---
### Healthcheck:
* The service will fail a healthcheck if either of the BHQ Stats or Player service cannot be reached.