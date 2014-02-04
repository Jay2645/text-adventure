local bruce
local states =
{
	hello =
	{
		allowedStates =
		{
			"howareyou"
		},
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
		allowedStates =
		{
			"doingfine"
		},
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

function addplayers(room)
	bruce = room:AddCharacter("Bruce")
	bruce.state = main:CreateState("hello")
	current = states.hello
	for i=1, #(current.allowedStates) do
		bruce.state:AddAllowedState(states.hello.allowedStates[i])
	end
end

function onenter(room)
	if(current == states.hello) then
		bruce:Say("Oh, hello there, mate.")
	elseif(bruce:CheckGoal()) then
		bruce:Say("Oh, fantastic. You found my flowerpot.")
		main:GameOver("You won!")
	end
end

function onspeak(speech)
	for key,value in pairs(current.stateTransitions) do
		if string.find(speech,key) and bruce.state:IsAllowedState(value) then
			bruce:Say(current.response)
			current.func()
			current = states[value]
			if current then
				bruce.state = main:CreateState(value)
				for i=1, #(current.allowedStates) do
					bruce.state:AddAllowedState(current.allowedStates[i])
				end
			end
		end
	end
end

start:BindMessageFunction(addplayers,"onroominit")
start:BindMessageFunction(onenter,"onroomprint")
player:BindMessageFunction(onspeak,"oncharacterspeak")
