# Bachelor Thesis Exploded Views
 This is the main repository for my bachelor's thesis on Occlusion Avoidance for Immersive Inspection of 3D Cell Complexes and Cell Surfaces

----

## How to use:
1. Download the latest release.
2. Get a data set:
- Data sets can be found in this *[.zip](https://github.com/JorGra/Bachelor-Thesis-Exploded-Views/blob/a7f230f26ccc69423c1d7bf68cc2e698784ee87a/Thesis/BA-%20ExplodedView/Assets/Resources/Datasets.zip)* archive. Unzip and place one of them inside the "Resource"-folder of the program.  

- or by simulating one using Morpheus.
3. Start the BA- ExplodedView.exe
4. The scene should now load. On your left hand is a UI panel, that can be used to change explosion parameters and the implementation used.

*Note that some parameters can only be adjusted by running the program from the editor.*

----

## Methods:

Switch between the implementations by clicking on the icon in on top of the center UI panel.

### Point explosion

Explode all parts away from the control point.   

<img src="https://github.com/JorGra/Bachelor-Thesis-Exploded-Views/blob/1f113387de45282433bbf92ea9d280cdb4d7b454/Thesis/latex-template-cgv/fig/Images/PointExplosionPicture.png" width="60%">

### Line explosion

Define a line by positioning the two control points in the scene. Parts are then projected on this line and exploded opposit to this projection point.

<img src="https://github.com/JorGra/Bachelor-Thesis-Exploded-Views/blob/1f113387de45282433bbf92ea9d280cdb4d7b454/Thesis/latex-template-cgv/fig/Images/LineExplosionPictures.png" width="100%">

### Head-mounted explosion

This extends the line explosion to be view-dependent

<img src="https://github.com/JorGra/Bachelor-Thesis-Exploded-Views/blob/1f113387de45282433bbf92ea9d280cdb4d7b454/Thesis/latex-template-cgv/fig/Images/HeadMountedLineExplosion.png" width="100%">

### Force-Based explosion

Uses forces to generate the explosion view. Multiple parts can be selected by grabbin a cell and pressing the triggerbutton.

<img src="https://github.com/JorGra/Bachelor-Thesis-Exploded-Views/blob/1f113387de45282433bbf92ea9d280cdb4d7b454/Thesis/latex-template-cgv/fig/Images/ForceBasedExplosionPicture.png" width="100%">





