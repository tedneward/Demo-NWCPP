from algorithm.functional import parallelize

# "parallelize" is documented as:
#
# parallelize[origins: origin.set, //, func: fn(Int) capturing -> None](num_work_items: Int)
#
# recall that "[]" parameters are compile-time parameters, and that
# "//" means that any parameter to the left of that symbol are inferred, not explicitly
# passed. I have no freakin clue what "origin.set" is, but it turns out I don't have
# to because it's inferred. Yay! However, the "func" function *must* be declared as a
# "fn", not "def", because... reasons? Also not sure what the "capturing" suffix does.
#
# The parameter passed to "func" on execution appears to be a thread identifier or
# something; when I print it, it appears to be the sequence of values 0-100 but in some
# kind of random-ish order, which would correspond well to the idea of launched parallel
# sequences of execution (threads or otherwise).
#

# This approach works surprisingly well; a few runs and we get results
# of 99000, 100000 and 100000 (which is astonishingly correct). I could never
# get that 99000 result again, but I don't call this a success because I did see it
# that one time.
#
def first_pass():
    collective_count = 0

    fn thread_count(thread_count : Int) capturing -> None:
        count = 0
        for _ in range(1000):
            count += 1
        collective_count += count

    parallelize[thread_count](100)

    print(collective_count)

    print("All done")


# This approach works surprisingly well; a few runs and we get results
# of 96280, 96635, and 100000 (which is astonishingly correct).
#
def second_pass():
    collective_count = 0

    fn thread_count(thread_count : Int) capturing -> None:
        for _ in range(1000):
            collective_count += 1

    parallelize[thread_count](100)

    print(collective_count)

    print("All done")

# So now let's try to synchronize access to collective_count so that we don't lose the
# few updates that we lost.
#
def main():
    print("Hello world")

    collective_count = 0

    fn thread_count(unknown : Int) capturing -> None:
        for _ in range(1000):
            collective_count += 1

    parallelize[thread_count](100)

    print(collective_count)

    print("All done")
