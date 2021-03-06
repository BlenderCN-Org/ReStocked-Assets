# ReStocked-Assets
My own Assets made for [ReStock](https://github.com/PorktoberRevolution/ReStocked). Please report any issues there. Models are made in Blender 2.79 and textures are made with Photoshop CC and Affinity Designer. All files are licensed CC-BY-SA 4.0. 

#### Naming Convention:
textures are named with the filename they will appear as in the final distributed version of the mod, with the exception of some specular maps which are copied into the alpha channel of the diffuse textures. For example:
* `restock-sciencebox-1`: Diffuse map
* `restock-sciencebox-1-s`: Specular map, copied into alpha channel of Albedo map except when the Specular (Mapped) shader is used
* `restock-sciencebox-1-spec`: Specular map, but gets automatically copied into the alpha channel on import into unity (used with Affinity Designer workflow)
* `restock-sciencebox-1-n`: Normal map, makes use of Smart Objects for the heightmap embedded within
* `restock-sciencebox-1-e`: Emissive map

#### Other Work
* Uses [High voltage warning.svg](https://commons.wikimedia.org/wiki/File:High_voltage_warning.svg) by various wikipedia contributors, licensed under the [Creative Commons Attribution-Share Alike license](https://creativecommons.org/licenses/by-sa/2.5/deed.en). It has been modified when embedding into textures for pixel-accuracy
