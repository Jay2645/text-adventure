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
		response = "How are you doing today, then?"
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
		response = "That's good, that's good."
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
	bruce:Say("Oh, hello there, mate.")
end

function onspeak(speech)
	for key,value in pairs(current.stateTransitions) do
		if string.find(speech,key) and bruce.state:IsAllowedState(value) then
			bruce:Say(current.response)
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
