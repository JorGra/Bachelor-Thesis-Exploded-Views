# Bachelorarbeit 

## Paper

### Compact Explosion Diagrams
https://dl.acm.org/doi/pdf/10.1145/1809939.1809942?casa_token=NCsKiYcJmhsAAAAA:uG5cVFgq4wXQoaD0BtvBCM50F29qwagScCxXW_tlDVXjaEswW9IYe93gZCPeOh0ScG3ZvEs0jvH9

Markus Tatzgern et al. describe a technique to find common reoccuring subsets of a mesh and then chose a representative of that group to displace based on a quality rating. This is useful for mechanical meshes with many similar parts. The reoccuring subsets are found automatically by using a frequent subgraph search(FSG). This won't be useful for my work as it focuses on biological datasets that doesn't feature many similar parts. 
In their work they show different possibilities on how to displace parts to maximize the information that is shown and to minimize the visual noise by the parts that are shown in the background. 
These two possibilities are achiceved by tweeking the qualitiy rating to favor (a) the visibility of the representative group, therefore maximizing the amount of screen space the exploded view covers or (b) chosing the dot product of the viewing direction and the explosion direction. 
The quality meassure of the selected representative takes the following attributes into account:
- *Size of the footprint of the exploded view:* overall screen space of the exploded view
- *Visibility of parts of the exploded group:* decribed as the relative meassure for the general visibility of the parts
- *Part directions relative to current camera viewpoint:* assumes that explosions similar to viewing direction are more difficult to read, they compute the average dot product between the viewing angle and the explosion direction 
- *Size of footprint of all other similar groups without any displacements* describes how well other similar groups are visible when selected representative is exploded

These attributes are then weighted and the best representative is exploded. While this won't translate to the virtual space, some ideas are insightful: trying to maximize the dot product of the viewing direction  and explosion direction seems useful for, especially of the application is used in seated mode as it gives a better initial explosion direction. Additionally could a quality score like this be used as a good reference between a good exlpoded view and a mediocre one and for user studies.



### Medical Image Atlas Interaction in Virtual Reality
https://aviz.fr/~bbach/immersive2017/papers/IA_1371-paper.pdf

Lindun He et al. desribe the use of VR devices to explore medical data of a brain model in CAD format. They calculate the axis-aligned bounding boxes of each mesh, then build a complete bunding volume hierarchy from the bottom-up. This is neccessary as medical data doesn't have a specific assembly order that can be revealed, in their paper they use the parent-child relationships to constrain the transformation.
To allow easy manipulation of the exploded view axis they use hermitsplines that can be drawn with vr-controllers. They also describe multiple ways of interacting with the mesh to trigger different explosion views:
- *linear explosion:* simply spreading the controllers, transforms the mesh
- *leafing interaction:* cut object into pieces and search trough them as if they were pages in a book
- *fanning interaction:* like spreading a hand of playing cards so that the suits of the cards are visible


### Automated Generation of Interactive 3D Exploded View Diagrams
https://dl.acm.org/doi/pdf/10.1145/1360612.1360700

Wilmot Li et al. describe a system that automatically extracts a non-blocking exploded view of a 3D model, they focus on rearanging parts compared to removing occluding geometry. They also provide a list of tools to interact with the exploded model to interactively show parts of interest.
They focus on linear exploded views and present a general list of desired attributes for exploded views: 
- *Blocking constraints:* parts are exploded away from each other in unblocking directions, to help the viewer understand the local blocking parts of the model and their relative position
- *visibility:* the offset should be choses so that all parts of interest are visible
- *Compactness:* Exploded views should minimize the  distance from their original position to make it easier for the viewer to mentally reconstruct the model
- *Canonical explosion directions:* Many objects have a canonical coordinate frame that may be defined by a number of factors, including symmetry, real-world orientation, and domainspecific conventions. In most exploded views, parts are exploded only along these canonical axes. Restricting the number of explosion directions makes it easier for the viewer to interpret how each part in the exploded view has moved from its original position.
- *Part hierarchy:* subassemblies can be used to show smaller collections of parts

They also describe to common ways of cutting objects to show internal parts:
- *Splitting containers:* cut containers in two halfs to show nested parts, orientation sometimes differs from the one used for the internal parts
- *Contextual cutaways:* are used to show the original position of an exploded view inside the original model, while showing the exploded view on the outside to give a detailed view to the user

They continue showing an algorithm that automatically generates exploded view from hierarchical and non-hierarchical CAD models. Their approach works by generating an explosion graph when the model is loaded. This graph contains information that is needed to determin how the model should be expanded based on the current view direction. It allows the algorithm to determin the order in which the parts should be moved dynamically. One downside is that this apporach only works on rigid models that do not change their shape over time and it needs to be calculated in advance. They also describe how containers can be split by cutting the bounding boxes of the mesh in half and moving the two parts away so that none of the inside parts are occluded. 


### Exploded View Diagrams of 3D Grids
DOI: 10.1109/SIBGRAPI.2015.12

### Exploded Views for Volume Data
10.1109/tvcg.2006.140

### Using Deformations for Browsing Volumetric Data
https://www.dgp.toronto.edu/papers/mmcguffin_IEEEVIS2003.pdf


## Types of exploded views/occlusion avoidance
- linear transformation
- fanning, leafing views
- peel away layers using non-rigid deformations
- 2D fisheye techniques to enlarge parts of interest
- force based model to push occluding parts out of the way
- magic lenses 

-----
## Possible structure

### Introduction
#### Motivation
Here you motivate, why you are doing your research.
Which problem does this thesis try to solve and how? What is new? Why is it relevant? What is the strategy to solve this problem? Which parts of the problem are being worked on, which are excluded? 
Why is occlusion a problem, when viewing models and datasets in general? 
Why is the virtual reality a good way to visualize data? What does it offer compared to the traditional desktop screens?
Why is the inspection of surfaces in the virtual space advantageous?

#### Goal 

This section should clarify, what should be achieved by the work.
How does my implementation solve the occlusion problem?
How do I plan on implementing an exploded view in VR? What is the overall goal for this thesis?
Which questions and task am I trying to answer/solve?

#### Structure of work 
Here the structure can be *briefly* explained.
Possible structure:
- related work, maybe do it chronologically  like Li et al. 
- background/basic concepts/important attributes of exploded views should have
- describe different implementation concepts and categorize them
- explain how I implemented those concepts in my work, which technologies did I use and why, what did I change and why and what did I do that is somewhat new
- Show results of my work/what went right and what did not work
- discuss results, what went well, what was interesting about my implementation and why? Why did some things not work and what could be improved and how? 
- Summarize results and draw a conclusion of the work 
- give an outlook on what could be done to improve the work

### Related work
chronologically summarize related work and papers on the subject of occlusion avoidance

### Background
Which basic concepts does the reader need to understand this thesis?
Explanation of important attributes exploded views should have?

### Methods
#### introductions to this chapter and comparison of 2D exploded views and exploded views in VR  
**Theoretical analysis** 

start by comparing the characteristics of the virtual space to 2D screens and the differences in the interaction
- viewing angles 
- depth in selection
- movement in the scene/data set
- occlusion avoidance

What are important traits for useful/good exploded views? 
How can we categorize them?
*Differentiate between view dependend/view independend exploded views*

#### Possible implementations
**practical realisation**

describe different implementation concepts and categorize them, what advantages do they have? What do they require?

- *simple linear explosion away from given point*
- *Cone/Cylinder- shape explosion*
- *view definded/dependend explosion*
- *anti-gravity view*

How can the I use the advantages of vr interaction to implement a good exploded view/avoid occlusion?
Which user interactions are supported and why?

### Results
Show results of the work, this might be difficult for this project as evalutation of exploded views are somewhat subjective
a user study might be useful here, but I don't know if i'll have the time for that.
If not compare results with the meassurement that has been established in the previous section.
Quantitative/Qualitative evaluation if that is possible.

### Discussion
Review results and explain advantages and disadvatages of the implemented exploded views.
What went well what could be improved?

### Summarization
shortly summarize the thesis and what has been done
give a short conclusion and explain briefly how the questions in the motivation have been answered
- *how has occlusion avoidance been implemented and improved using vr technology in this thesis*
- *which conclusions are to be drawn from the work*


### Outlook
What could be done to further improve the concepts that have been implemented and work on?
Which weekpoints does the current implementation have that would need to be fixed?
Which further reserch could be done on the subject? Possible other solutions to the problem that haven't been explored in this thesis?


-----
## implementation ideas
![implementation ideas](/Thesis/Resources/wip/scetches/implementaion_ideas.jpg)


## Fragen
- how could a quantitative/qualitative evaluation look like?

