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
    
    target_scale: bpy.props.FloatVectorProperty(
        name="Target Scale",
        default=(1.0, 1.0, 1.0),
        subtype='XYZ'
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
        
        
        # Target Coordinate Space Column
        space_box = panel_col.box()
        space_col = space_box.column()
        
        space_col.label(text = "Target Coordinate Space")
        space_col.prop(my_props, "target_space", expand = True)
        
        
        # Target Scale Column
        panel_col.label(text = "Target Scale")
        scale_col = panel_col.column(align=True)
        scale_col.prop(my_props, "target_scale", index=0, text="X")
        scale_col.prop(my_props, "target_scale", index=1, text="Y")
        scale_col.prop(my_props, "target_scale", index=2, text="Z")
        
        
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
        target_scale = my_props.target_scale
        
        # Get the active object
        obj = bpy.context.view_layer.objects.active
        mesh = obj.data
        
        # Create a BMesh from the mesh data
        bm = bmesh.new()
        bm.from_mesh(mesh)
        
        bmesh.ops.scale(
            bm,
            vec=target_scale,           # Scale factor for each axis
            space=obj.matrix_world,     # Transformation matrix
            verts=bm.verts,             # Optional list of vertices to scale
        )
        
        # Triangulate the BMesh if necessary
        if not mesh_only_triangles(mesh):
            bmesh.ops.triangulate(bm, faces=bm.faces[:])
        
        
        data = "CL3D_KEY" + "\n"
        
        data += "VERTICES" + "\n"
        if target_space == "BLENDER":
            for vert in bm.verts:
                data += f"{vert.co.x} {vert.co.y} {vert.co.z}\n"
        if target_space == "UNITY":
            for vert in bm.verts:
                data += f"{vert.co.x} {vert.co.z} {vert.co.y}\n"
        data += "ENDVERTICES" + "\n"
        
        
        data += "TRIANGLES" + "\n"
        for face in bm.faces:
            if target_space == "BLENDER":
                verts = [str(v.index) for v in face.verts]
            elif target_space == "UNITY":
                verts = [str(v.index) for v in reversed(face.verts)]
            verts_str = " ".join(verts)
            data += verts_str + " "
        
        # Trim the trailing space from the string
        data = data[:-1]
        data += "ENDTRIANGLES" + "\n"
        
        data += "METADATA" + "\n"
        
        data += "ENDMETADATA"
        
        
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