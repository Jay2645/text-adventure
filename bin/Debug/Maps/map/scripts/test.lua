function onenter(room)
	room:AddCharacter("Bruce")
end

start:BindMessageFunction(onenter,"onroomenter")
