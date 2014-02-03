function onenter(room)
	bruce = room:AddCharacter("Bruce")
	bruce:Say("Croikey, m8, is it 'ot in 'ere or what?")
end

function onspeak(speech)
	if string.find(speech, "hello") then
		bruce = main:GetCharacter("Bruce")
		bruce:Say("Oh, why 'ello to you, too, m8!")
	end
end

start:BindMessageFunction(onenter,"onroomprint")
player:BindMessageFunction(onspeak,"oncharacterspeak")
