local bruce
local states =
{
	hello =
	{
		stateTransitions =
		{
			hello = "howareyou",
			hi = "howareyou"
		},
		response = "Listen, can you get my flowerpot?",
		func = function() bruce:SetAquireGoal("flowerpot","Player") end
	},
	howareyou = 
	{
		stateTransitions = 
		{
			fine = "doingfine",
			good = "doingfine",
			well = "doingfine"
		},
		response = "That's good, that's good.",
		func = function() end
	}
}
local current

local function allowstates(character, state)
	current = states[state]
	character.state = main:CreateState(state)
	for key,value in pairs(current.stateTransitions) do
		character.state:AddAllowedState(value)
	end
end

local function addplayers(room)
	bruce = room:AddCharacter("Bruce")
	allowstates(bruce, "hello")
end

local function onenter(room)
	if(current == states.hello) then
		bruce:Say("Oh, hello there, mate.")
	elseif(bruce:CheckGoal()) then
		bruce:Say("Oh, fantastic. You found my flowerpot.")
		main:GameOver("You won!")
	end
end

local function onspeak(speech)
	for key,value in pairs(current.stateTransitions) do
		if string.find(speech,key) and bruce.state:IsAllowedState(value) then
			bruce:Say(current.response)
			current.func()
			if current then
				allowstates(bruce, value)
			end
		end
	end
end

local function test(param)
	print("Hi there!")
	print(param)
end

start:BindMessageFunction(addplayers,"onroominit")
start:BindMessageFunction(onenter,"onroomprint")
player:BindMessageFunction(onspeak,"oncharacterspeak")
main:AddCommand("testing", "Tests the Lua messaging system.", test)
