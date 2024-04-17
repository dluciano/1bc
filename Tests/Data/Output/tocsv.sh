exec sed '
#
# Transform calculate_average*.sh output into semicolon-separated values, one per line.
#

# 1. remove "{" and "}"
s/[{}]//g;

# 2. replace "=" and "/" with semicolon
s/[=/]/;/g;

# 3. id may contain comma, e.g. "Washington, D.C.;-15.1;14.8;44.8, Wau;-2.1;27.4;53.4"
# so replace ", " with a newline only if it is preceded by a digit
s/\([0-9]\), /\1\n/g
'