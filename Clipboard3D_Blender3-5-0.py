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
        properties = scene.properties
        
        col = layout.column()
        col.operator("cl3d.copy")
        
        box = col.box()
        box.label(icon = "OUTLINER_OB_LIGHT", text="Copies data to clipboard")
        
        
        
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
    
# ------------------------------------------------------------------------
#    Klasser
# ------------------------------------------------------------------------

class CL3D_Copy(bpy.types.Operator):
    bl_idname = "cl3d.copy"
    bl_label = "Get Model"
    bl_description = "Copies the active mesh object as CL3D data to the clipboard"
    
    def execute(self, context):
        #my_props = context.scene.properties     # Properties
        
        # Get the active object
        obj = bpy.context.view_layer.objects.active
        mesh = obj.data
        
        # Create a BMesh from the mesh data
        bm = bmesh.new()
        bm.from_mesh(mesh)
        
        # Triangulate the BMesh
        bmesh.ops.triangulate(bm, faces=bm.faces[:])
        

        data = "CL3D"
        
        data += "\n" + "VERTICES"
        for vert in bm.verts:
            data += "\n" + str(vert.co.x) + " " + str(vert.co.y) + " " + str(vert.co.z)
        data += "\n" + "ENDVERTICES"
        
        
        data += "\n" + "TRIANGLES"
        data += "\n"
        for tri in bm.faces:
            for vert in tri.verts:
                data += str(vert.index) + " "
        
        # Get rid of the last space among the indices
        data = data[:-1]
        data += "\n" + "ENDTRIANGLES"
        
        
        
        bpy.context.window_manager.clipboard = data;
        print(data)
        
        
        
        return{"FINISHED"}

# ------------------------------------------------------------------------
#    Registrering
# ------------------------------------------------------------------------

classes = [MyPropertyGroup, CL3D_Panel, CL3D_Copy]


def register():
    for cls in classes:
        bpy.utils.register_class(cls)
        
    bpy.types.Scene.properties = bpy.props.PointerProperty(type=MyPropertyGroup)
        
    
def unregister():
    for cls in classes:
        bpy.utils.unregister_class(cls)
    del bpy.types.Scene.properties
    


if __name__ == "__main__":
    register()