\ This is a prime calculator program

: setup ( n1 -- a-addr n2 )
  cells allocate throw  \ allocate n1 cells of memory
  2 over !	 	\ store 2 in the beginning of the sieve
  1 	  		\ one element in the sieve
;

: teardown ( a-addr -- )
  free throw 	       \ free the allocated memory
;

: checksieve ( a-addr n1 n2 -- n1 f ) recursive \ checks if n1 is a multiple of the n2 primes already stored in a-addr
dup 0 > if      \ if we don't have any primes in the (remaining) sieve, stop
  swap rot over \ primes candidate address candidate
  over @   	\ copy address to top and read
  mod 		\ push modulo calculation onto stack
  0 > if	\ if != 0
    1 cells +	\ calculate offset address for recursive call
    rot 1 - 	\ decrement amount of primes to check
    rot swap	\ restore order
    checksieve  \ recursive call
  else
     rot drop drop true \ clean up after ourselves
  endif
else
   over dec.		\ this is were we actually report each found prime
   rot drop drop false	\ clean up after ourselves
endif
;

: runhelper ( a-addr n1 n2 -- ) recursive \ n1 number of cells in a-addr, n2 number of already occupied cells
2dup <> if     	     	\ break off the recursion if we have as many primes as cells
   rot 2dup 2dup 2dup	\ make enough copies of address and cell number
   swap	    	 	\ rearrange to allow for cell access
   1 -			\ decrement index by 1
   cells + @		\ read last cell
   1 +			\ generate candidate from last cell
   rot			\ rearrange arguments to befit checksieve
   checksieve		\ check if our candidate is a multiple of a known prime or not
   begin
     0 <> while		\ while candidate isn't prime
     1 +  		\ increment candidate by one
     1 2over 2tuck 2drop drop	    \ FIXME, this is fucking ugly but required to copy our addresses and prime counts so far
     rot		\ rearrange arguments to befit checksieve
     checksieve		\ check if our candidate is a multiple of a known prime or not
   repeat		\ repeat until we found an unknown prime
   swap rot		\ arrange arguments so we can store it in the cell
   cells + !		\ store
   swap 1 + swap	\ increment number of found primes
   rot rot  		\ rearrange arguments for recursive call
   runhelper  		\ recursive call
endif
;

: run ( n -- )	\ n is the number of primes to find
  dup 	     	\ duplicate that number
  setup		\ allocate n cells 
  rot swap	\ prepare args for runhelper
  runhelper	\ run runhelper
  rot		\ move address to top
  teardown	\ free the memory at address
;

100 run		\ calculate 100 primes





