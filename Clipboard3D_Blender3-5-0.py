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
#from math import radians, degrees

# ------------------------------------------------------------------------
#    Properties
# ------------------------------------------------------------------------

class MyPropertyGroup(bpy.types.PropertyGroup):
    
    """
    decimals : bpy.props.IntProperty(name = "", min=0, soft_max=4)
    multiple : bpy.props.FloatProperty(name = "Value", min=0)
    
    include_location : bpy.props.BoolProperty(name = "Location")
    include_rotation : bpy.props.BoolProperty(name = "Rotation")
    include_scale : bpy.props.BoolProperty(name = "Scale")
    """
    

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
        my_props = scene.myProperties
        
        # Active Object Information
        alive = len(context.selected_objects) != 0
        
        if alive:
            obj = bpy.context.view_layer.objects.active
            mesh = obj.data
        
        
        col = layout.column()
        
        # Copy Model Operator
        row = col.row()
        row.enabled = False
        
        if alive and obj.type == "MESH":
            row.enabled = True
            
        row.operator("cl3d.copy", icon = "COPYDOWN")
        
        
        # Triangulation Warning
        row = col.row()
        
        if alive and obj.type == "MESH":
            if not mesh_only_triangles(mesh):
                box = row.box()
                box.label(icon = "OUTLINER_OB_LIGHT", text = "Mesh will be triangulated")
        
        
        
        """
        row = layout.row()
        row.label(text = "Snap To")
        
        row = layout.row()
        row.prop(properties, "snapping_mode", expand=True, text = " ")
        
        if properties.snapping_mode == "decimal":
            box = layout.box()
            split = box.split(factor=0.5)
            split.scale_y = 2
            col = split.column()
            col.label(text = "Precision")
            col = split.column()
            col.prop(properties, "decimals")
            
        elif properties.snapping_mode == "multiple_of":
            box = layout.box()
            split = box.split(factor=0.5)
            split.scale_y = 2
            col = split.column()
            col.label(text = "Value")
            
            col = split.column()
            col.prop(properties, "multiple")
        
        box = layout.box()
        row = layout.row()
        box.label(text = "Affect")
        
        col = box.column(align = True)
        col.prop(properties, "include_location", icon = "OBJECT_ORIGIN")
        col.prop(properties, "include_rotation", icon = "ORIENTATION_GIMBAL")
        col.prop(properties, "include_scale", icon = "EMPTY_ARROWS")
        
        row = layout.row()
        row.scale_y = 2
        row.operator("object.snap_selected")
        
        row = layout.row()
        
        if properties.active_snapping: icon = "SNAP_ON"
        else: icon = "SNAP_OFF"
            
        #row.prop(properties, "active_snapping", icon = icon)
        """

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
#    Klasser
# ------------------------------------------------------------------------

class CL3D_Copy(bpy.types.Operator):
    bl_idname = "cl3d.copy"
    bl_label = "Copy Model"
    bl_description = "Copies the active mesh object as CL3D data to the clipboard"
    
    def execute(self, context):
        #my_props = context.scene.myProperties     # Properties
        
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
            data += "\n" + str(vert.co.x) + " " + str(vert.co.y) + " " + str(vert.co.z)
        data += "\n" + "ENDVERTICES"
        
        
        data += "\n" + "TRIANGLES"
        data += "\n"
        for tri in bm.faces:
            for vert in tri.verts:
                data += str(vert.index) + " "
        
        # Trim the trailing space from the string
        data = data[:-1]
        
        data += "\n" + "ENDTRIANGLES"
        
        
        bpy.context.window_manager.clipboard = data;
        return{"FINISHED"}

# ------------------------------------------------------------------------
#    Registrering
# ------------------------------------------------------------------------

classes = [MyPropertyGroup, CL3D_Panel, CL3D_Copy]


def register():
    for cls in classes:
        bpy.utils.register_class(cls)
        
    bpy.types.Scene.myProperties = bpy.props.PointerProperty(type=MyPropertyGroup)
        
    
def unregister():
    for cls in classes:
        bpy.utils.unregister_class(cls)
    del bpy.types.Scene.myProperties
    


if __name__ == "__main__":
    register()