import ast
import json
import pandas as pd
import seaborn as sns
import numpy as np
import matplotlib.pyplot as plt
data_list = []
with open("outputData.txt", "r") as f:
    for line in f:
        if line.strip() == '[' or line.strip() == ']':
            continue
        else:
            while line[-1] != '}' :
                line = line[:-1]
            res = ast.literal_eval(line)
            data_list.append(res)

df = pd.DataFrame(data_list)

def plotGraph(gene_param):
    grouped_multiple = df.groupby(['generation', 'species']).agg({gene_param: ['max']})
    grouped_multiple.columns = [gene_param]
    grouped_multiple = grouped_multiple.reset_index()

    fig, ax = plt.subplots()

    for key, grp in grouped_multiple.groupby(['species']):
        ax = grp.plot(ax=ax, kind='line', x='generation', y=gene_param,label=key)

    max_value = grouped_multiple['generation'].max()
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