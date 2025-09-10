# 20250907_noise_notes
## About
* aaaa the noise?? notes?? 
## Float based deterministic noise
* so usually they say to use float values and then just a lil simple math equation
* but then they try to use trig which is spooky (`sin(..)`), so we want to avoid that if we can
### removing trig
#### OPTION 1: integer array hashing
* we could yoink the permutation method used in Ken's original Perlin noise paper
    - then just have the CPU supply these values which it randomises
    - would also let us specify the seed/initial value to go in to the system
    - less float magic, more integer magic (may end up faster on older hardware? dont newer gpus prefer floats now?)
#### OPTION 2: using [dot product](https://en.wikipedia.org/wiki/Dot_product)/[cross product](https://en.wikipedia.org/wiki/Cross_product) instead
* dot product is `||A|| * ||B|| * cos(theta)`
* cross product is `||A|| * ||B|| * sin(theta) * n`
* `theta` is the angle between vectors `A` and `B`
* `n` being the vector perpendicular to the vectors `A` and `B`
* we could further improve this by using squared magnitude so we dont need to use a costly square root operation
    - if it ends up being a spooky thing that matters, we can always do something like [fast inverse square root](https://en.wikipedia.org/wiki/Fast_inverse_square_root) and just remember how to flip the fraction so it's not the inverse?
    - maybe there's something in the way floats are obtained that we can use since they're polynomials and we get access to the bits for cheap multiply/divide operations
    - perhaps even just using [newton's method](https://en.wikipedia.org/wiki/Newton%27s_method) with a low iteration count (2 to 4)
## rule 30
* there was some stuff in the wolfram alpha something or rather ([rule 30](https://en.wikipedia.org/wiki/Rule_30)) that seemed somewhat useful
* making this 2D would be similar to how binary trees become quad trees, so we'd use `x` and `y` to specify which is being changed
* could be that the axis is like an array representation of the tree like with [heaps](https://en.wikipedia.org/wiki/Heap_(data_structure))
* over time it grows to envelop the whole image space
* could use a border around the edge to specify information like what iteration it is, that way we can save our important image in to an iteration's result
    - something like qr codes or a data matrix, but flattened in to a single pixel ring around the edge
    - perhaps some spacer rings so that cropping when mipmapping doesnt cry so much
    - this ends up being similar to how [CDs](https://en.wikipedia.org/wiki/Compact_Disc_Digital_Audio#Data_structure) use the lead-in/lead-out portion of the track to store their table of contents and error correction