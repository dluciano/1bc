INPUT="./*.out"
mkdir temp
for sample in $(ls $INPUT); do
  ./tocsv.sh < ${sample%} > ${sample%.txt}.csv
  mv ${sample%} /temp/${sample%}
done
