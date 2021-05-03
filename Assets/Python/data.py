import random
import json
import ast
import json
import pandas as pd
import seaborn as sns
import numpy as np
import matplotlib.pyplot as plt
file = open('../geneList.json')
content = json.loads(file.read())
genes = content['genes']
# print(data)
random.seed()

def mutate(lb, ub, val):
    a =   100
    b =  -100
    r = random.uniform(a, b)
    return val+ (val * r/100)

res =[]
# listOfVal={};

block = [[15, 8, 12, 10, 15, 16, 15, 8, 12, 10, 15, 20, 18, 26, 21, 19, 24, 28, 29, 23, 27], [15, 8, 12, 10, 15, 15, 8, 12, 10, 15, 12, 10, 13, 17, 18, 23, 19, 21, 26, 29]]
lbs = [[10, 1, 4, 3, 5, -2 , 2, -5, -7, 5, 7, 4, 2, 8, 5, 6, 3, 8, 4, 6, 9], [30, -30, 80, 65, 35,10, 85, 90, 5, 7, 4, 6, 9, 6, 3, 7, 5, 8, 6, 5]]
ubs = [[6, 8, 10, 5, 9, 5, 11, 10, 5, 7, 5, 8, 8, 6, 9, 7, 9, 6, 9, 7, 8, 13], [50, -20, 90, 70, 45, 20, 95, 7, 5, 7, 10, 8, 9, 13, 8, 5, 9, 8, 11, 10, 8]]

for d in genes:
    for k in block:
        #print(block.index(k))
        for genCount in k:
            #print(k.index(genCount))
            for j in range(0,genCount):
                obj = {}
                obj['generation'] = k.index(genCount)
                obj['species'] = d['species']
                # obj['organismType'] = d['organismType']
                # obj['dietType'] = d['dietType']
                i = random.randrange(1, 3, 1)
                if i == 1:
                    obj['gender'] = "MALE"
                else:
                    obj['gender'] = "FEMALE"
                obj['range'] = mutate(lbs[block.index(k)][k.index(genCount)], ubs[block.index(k)][k.index(genCount)] , d['range'])
                obj['angle'] = mutate(lbs[block.index(k)][k.index(genCount)], ubs[block.index(k)][k.index(genCount)] , d['angle'])
                obj['sprintSpeed'] = mutate(lbs[block.index(k)][k.index(genCount)], ubs[block.index(k)][k.index(genCount)] , d['sprintSpeed'])
                obj['walkSpeed'] = mutate(lbs[block.index(k)][k.index(genCount)], ubs[block.index(k)][k.index(genCount)] , d['walkSpeed'])
                obj['maxHP'] = mutate(lbs[block.index(k)][k.index(genCount)], ubs[block.index(k)][k.index(genCount)] , d['maxHP'])
                obj['maxStamina'] =mutate(lbs[block.index(k)][k.index(genCount)], ubs[block.index(k)][k.index(genCount)] , d['maxStamina'])
                obj['attack'] = mutate(lbs[block.index(k)][k.index(genCount)], ubs[block.index(k)][k.index(genCount)] , d['attack'])
                obj['attackGap'] =mutate(lbs[block.index(k)][k.index(genCount)], ubs[block.index(k)][k.index(genCount)] , d['attackGap'])
                obj['attackRange'] = mutate(lbs[block.index(k)][k.index(genCount)], ubs[block.index(k)][k.index(genCount)] , d['attackRange'])
                obj['defense'] = mutate(lbs[block.index(k)][k.index(genCount)], ubs[block.index(k)][k.index(genCount)] , d['defense'])
                obj['scale'] = mutate(lbs[block.index(k)][k.index(genCount)], ubs[block.index(k)][k.index(genCount)] , d['scale'])
                obj['urgeRate'] = mutate(lbs[block.index(k)][k.index(genCount)], ubs[block.index(k)][k.index(genCount)] , d['urgeRate'])
                obj['evadeCooldown'] = mutate(lbs[block.index(k)][k.index(genCount)], ubs[block.index(k)][k.index(genCount)] , d['evadeCooldown'])
                obj['children'] = random.randint(0, 3)
                res.append(obj)
    
               
        

with open('../outputData.json','w') as f:
    json.dump(res,f)

df = pd.DataFrame(res)

def plotGraph(gene_param):
    grouped_multiple = df.groupby(['generation', 'species']).agg({gene_param: ['max']})
    grouped_multiple.columns = [gene_param]
    grouped_multiple = grouped_multiple.reset_index()

    fig, ax = plt.subplots()

    for key, grp in grouped_multiple.groupby(['species']):
        ax = grp.plot(ax=ax, kind='line', x='generation', y=gene_param,label=key)

    max_value = grouped_multiple['generation'].mean()
    plt.xlabel('Generation')
    plt.ylabel(gene_param)
    plt.title('Variantions in '+ gene_param +' with respect to Generation')
    plt.xticks(np.arange(0, max_value+1, 1))
    plt.legend(loc='best')
    plt.show()

i = 1  
while True:  
    param = input("Enter parameter or exit: ") 
    if(param == 'exit'):  
        break
    plotGraph(param)