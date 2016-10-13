---------------------------------------------------------------------------------------------------
--===============================================================================================--
--filename: moban_view.lua
--data:2016..
--author:pu
--desc:view 
--===============================================================================================--
---------------------------------------------------------------------------------------------------

local LuaHelper=LuaHelper
local CSNameSpace = CSNameSpace

local view=class(AssetScene,function(self,item_obj)
    AssetScene._ctor(self,"loadscene.u3d","LoadScene",true) 
    self.item_obj=item_obj
end)

function view:on_asset_load(key,asset)
    self.item_obj:register_property_changed(self.databind,self)
	print(" Load scene loaded")
    -- local refer = LuaHelper.GetComponent(self.root,CSNameSpace.ReferGameObjects) 
	-- self.refer = refer
end

function view:dispose()
    self.item_obj:register_property_changed(self.databind,nil)
    self._base.dispose(self)
 end

function view:databind(item_obj,property_name)
    -- if property_name == item_obj.get_npc_map_ack then
    --     self:show_npc_map(item_obj:get_server_source_npc_map())
    -- end
end

return view