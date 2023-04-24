bl_info = {
    "name" : "Clipboard3D",
    "author" : "",
    "description" : "An easy-to-integrate solution for copy/pasting polygonal 3D data between applications",
    "version" : (1,0),
    "blender" : (3,5,0),
    "support": "COMMUNITY",
    }

import bpy
import bmesh
import numpy as np
#from math import radians, degrees

# ------------------------------------------------------------------------
#    Properties
# ------------------------------------------------------------------------

class CL3D_Properties(bpy.types.PropertyGroup):
    
    """
    decimals : bpy.props.IntProperty(name = "", min=0, soft_max=4)
    multiple : bpy.props.FloatProperty(name = "Value", min=0)
    
    include_location : bpy.props.BoolProperty(name = "Location")
    include_rotation : bpy.props.BoolProperty(name = "Rotation")
    include_scale : bpy.props.BoolProperty(name = "Scale")
    """
    target_space : bpy.props.EnumProperty(
        name="Target Coordinate Space",
        items=[("BLENDER", "Blender", "Right-handed, Z is up"),
               ("UNITY"  , "Unity"  , "Left-handed, Y is up"),
               ("EMPTY"  , "Empty Matrix", "")],
        default="BLENDER"
    )
    
    metadata : bpy.props.StringProperty(name="Metadata", default="")

# ------------------------------------------------------------------------
#    Panel
# ------------------------------------------------------------------------

class CL3D_Panel(bpy.types.Panel):
    bl_idname = "CL3D_PT_panel"
    bl_label = "Clipboard3D"
    bl_category = "CL3D"
    bl_space_type = "VIEW_3D"
    bl_region_type = "UI"
    
    def draw(self, context):
        layout = self.layout
        scene = context.scene
        my_props = scene.cl3d_props
        
        # Active Object Information
        alive = len(context.selected_objects) != 0
        
        if alive:
            obj = bpy.context.view_layer.objects.active
            mesh = obj.data
        
        # Column
        panel_col = layout.column()
        
        # Space Box Column
        space_box = panel_col.box()
        space_col = space_box.column()
        
        space_col.label(text = "Target Coordinate Space")
        space_col.prop(my_props, "target_space", expand = True)
        
        
        # Copy Model Operator
        row = panel_col.row()
        row.enabled = False
        
        if alive and obj.type == "MESH":
            row.enabled = True
            
        row.operator("cl3d.copy", icon = "COPYDOWN")
        
        
        # Triangulation Warning
        row = panel_col.row()
        
        if alive and obj.type == "MESH":
            if not mesh_only_triangles(mesh):
                box = row.box()
                box.label(icon = "OUTLINER_OB_LIGHT", text = "Mesh will be triangulated")
        

# ------------------------------------------------------------------------
#    Functions
# ------------------------------------------------------------------------

def round_to_multiple(number, multiple):
    return multiple * round(number / multiple)  #Avrundning till närmsta X av nummer

def mesh_only_triangles(mesh):
    """
    Returns True if the given mesh contains only triangles, False otherwise.
    """
    for polygon in mesh.polygons:
        if len(polygon.vertices) != 3:
            return False
    return True

# ------------------------------------------------------------------------
#    Classes
# ------------------------------------------------------------------------

class CL3D_Copy(bpy.types.Operator):
    bl_idname = "cl3d.copy"
    bl_label = "Copy Model"
    bl_description = "Copies the active mesh object as CL3D data to the clipboard"
    
    def execute(self, context):
        my_props = context.scene.cl3d_props     # Properties
        target_space = my_props.target_space
        
        # Get the active object
        obj = bpy.context.view_layer.objects.active
        mesh = obj.data
        
        # Create a BMesh from the mesh data
        bm = bmesh.new()
        bm.from_mesh(mesh)
        
        # Triangulate the BMesh if necessary
        if mesh_only_triangles(mesh) == False:
            bmesh.ops.triangulate(bm, faces=bm.faces[:])
        
        
        data = "CL3D_KEY"
        
        data += "\n" + "VERTICES"
        for vert in bm.verts:
            if target_space == "BLENDER":
                data += "\n" + str(vert.co.x) + " " + str(vert.co.y) + " " + str(vert.co.z)
            elif target_space == "UNITY":
                point = np.array([vert.co.x, vert.co.y, vert.co.z])
                transform_matrix = np.array([[1, 0, 0], [0, 0, 1], [0, 1, 0]])
                new_point = transform_matrix.dot(point)
                data += "\n" + str(new_point[0]) + " " + str(new_point[1]) + " " + str(new_point[2])
        
        data += "\n" + "ENDVERTICES"
        
        
        data += "\n" + "TRIANGLES"
        data += "\n"
        for tri in bm.faces:
            if target_space == "BLENDER":
                for vert in tri.verts:
                    data += str(vert.index) + " "
            elif target_space == "UNITY":
                for vert in reversed(tri.verts):
                    data += str(vert.index) + " "
        
        # Trim the trailing space from the string
        data = data[:-1]
        data += "\n" + "ENDTRIANGLES"
        
        data += "\n" + "METADATA"
        
        data += "\n" + "ENDMETADATA"
        
        
        bpy.context.window_manager.clipboard = data
        
        return{"FINISHED"}

# ------------------------------------------------------------------------
#    Registration
# ------------------------------------------------------------------------

classes = [CL3D_Properties, CL3D_Panel, CL3D_Copy]


def register():
    for cls in classes:
        bpy.utils.register_class(cls)
        
    bpy.types.Scene.cl3d_props = bpy.props.PointerProperty(type=CL3D_Properties)
  

def unregister():
    for cls in classes:
        bpy.utils.unregister_class(cls)
    del bpy.types.Scene.cl3d_props


if __name__ == "__main__":
    register()