# shuffling
## about
* doc for some notes on shuffling so we can sleep on it
## notes
### idea
* something that given a number, maps it to another number in a deterministic range
* 1-1 both sets are the same length with all items being unique
* set `A` is the same as `B`, but `A[x] != B[x]`, they're in a different order
### swaps
* swaps are real, but kinda spooky time
* each pixel one by one, very slow
* might be fine if we do it as groups on groups on groups
* maybe it's choosing regions to cycle their values along an axis but only in that region
* this means we could do lots of them a lot faster, without relying on cpu operations to do it
* might mean double buffer is needed
* could let us change things over time
* maybe it's just one slice of a 3d volume at a time and each slice swap is when the rotation happens
* could use xor operation so it's faster swaps
* maybe just 4 slices in depth so we dont have to do much
* then we can have 2 images 
### cyclical group
* using some like, cryptography something something
* https://en.wikipedia.org/wiki/Cyclic_group
* idk?? this some prime numbers, powers, and modulo, look at it again later