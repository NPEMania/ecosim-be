import json
i=0
listGenes=[]
listCount=[]
while(i!=99):
    floatArray=[]
    species=input("Species Name ")
    organismType=int(input("Organism Type: 0 for plant... 1 animal "))
    dietType=int(input("Diet Type: 0 for plant... 1 animal...2 both "))
    rangee=float(input("Range: "))
    floatArray.append(rangee)
    angle=float(input("Angle "))
    floatArray.append(angle)
    sprintSpeed=float(input("Sprint Speed: "))
    floatArray.append(sprintSpeed)
    walkSpeed=float(input("Walk Speed "))
    floatArray.append(walkSpeed)
    maxHP=float(input("Max HP: "))
    floatArray.append(maxHP)
    maxEnergy=float(input("Max Energy: "))
    floatArray.append(maxEnergy)
    maxStamina=float(input("Max Stamina: "))
    floatArray.append(maxStamina)
    attack=float(input("Attack: "))
    floatArray.append(attack)
    attackGap=float(input("Attack Gap: "))
    floatArray.append(attackGap)
    attackRange=float(input("Attack Range: "))
    floatArray.append(attackRange)
    defense=float(input("Defense "))
    floatArray.append(defense)
    scale=float(input("Scale "))
    floatArray.append(scale)
    urgeRate=float(input("Urge Rate: "))
    floatArray.append(rangee)
    evadeCoolDown=float(input("Evade Cooldown: "))
    floatArray.append(urgeRate)
    lifespan=float(input("Lifespan: "))
    floatArray.append(lifespan)
    count=int(input("Number to spwan: "))
    dict={
        'species':species,'organismType':organismType,'dietType':dietType,
        'range':rangee,'angle':angle,
    'sprintSpeed':sprintSpeed,'walkSpeed':walkSpeed,'maxHP':maxHP,
    'maxEnergy':maxEnergy,'maxStamina':maxStamina,'attack':attack,
    'attackGap':attackGap,'attackRange':attackRange,'defense':defense,
    'scale':scale,'urgeRate':urgeRate,'evadeCooldown':evadeCoolDown,
    'lifespan':lifespan,
    'floatArray':floatArray}
    listGenes.append(dict)
    listCount.append(count)
    i=int(input("Enter 99 to exit else random val int"))

data = {'genes': listGenes}
dataCount={'genesCount':listCount}

with open('../geneList.json','w') as f:
    json.dump(data,f)

with open('../geneListCount.json','w') as f:
    json.dump(dataCount,f)